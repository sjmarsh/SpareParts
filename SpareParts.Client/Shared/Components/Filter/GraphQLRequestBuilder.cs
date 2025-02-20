using System.Text;
using Humanizer;
using SpareParts.Shared.Extensions;
using SpareParts.Shared.Models;

namespace SpareParts.Client.Shared.Components.Filter
{
    public interface IGraphQLRequestBuilder
    {
        GraphQLRequest Build<T>(List<FilterLine> filterLines, List<FilterField> filterFields, string? rootGraphQLField = null, PageOffset? pageOffset = null);
    }

    public class PageOffset
    {
        public PageOffset(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GraphQLRequestBuilder : IGraphQLRequestBuilder
    {
        const string _andFilterPrefix = " and: {";

        public GraphQLRequest Build<T>(List<FilterLine> filterLines, List<FilterField> filterFields, string? rootGraphQLField = null, PageOffset? pageOffset = null)
        {
            var isPagingEnabled = pageOffset != null;
            var filter = BuildQueryFilter(filterLines);

            var parentLevelFields = BuildParentLevelFilterFields(filterFields);
            var childLevelFields = BuildChildLevelFilterFields(filterFields);

            var fields = parentLevelFields + childLevelFields;

            var rootField = rootGraphQLField ?? $"{typeof(T).Name.ToLower()}s";
            var sortOrder = $", order:[{{{filterFields.First().Name.Camelize()}: ASC }}]";
            var filterPageOffset = pageOffset != null ? $", skip: {pageOffset.Skip}, take: {pageOffset.Take}" : "";
            var pagingItemsStart = isPagingEnabled ? "items {" : "";
            var pagingItemsEnd = isPagingEnabled ? @"} 
pageInfo {
    hasNextPage
}
totalCount
" : "";

            var graphQLRequest = new GraphQLRequest
            {
                query = $@"{{
                        {rootField} (where: {{ {filter}}}{sortOrder}{filterPageOffset}) {{
                            {pagingItemsStart}
                            {fields}
                            {pagingItemsEnd}
                        }}
                    }}"
            };

            return graphQLRequest;
        }

        private string BuildQueryFilter(List<FilterLine> filterLines)
        {
            var filter = "";
            if (filterLines != null && filterLines.Any())
            {
                filter = BuildQueryFilterComponents(filterLines.Where(f => string.IsNullOrEmpty(f.SelectedField.ParentFieldName)).ToList(), filter, true);

                var groupedChildFilterLines = filterLines.Where(f => !string.IsNullOrEmpty(f.SelectedField.ParentFieldName)).GroupBy(f => f.SelectedField.ParentFieldName);
                foreach (var grp in groupedChildFilterLines)
                {
                    var prefix = filter.Length > 0 ? _andFilterPrefix : "";
                    var childFilter = $" {prefix} {grp.Key.Camelize()}: {{ some: {{ ";
                    childFilter = BuildQueryFilterComponents(grp.ToList(), childFilter, false);
                    childFilter += "}}";
                    filter += childFilter;
                }

                // build end braces
                for (int i = 0; i < filterLines.Count - 1; i++)
                {
                    filter += "}";
                }
            }
            return filter;
        }

        private string BuildQueryFilterComponents(List<FilterLine> filterLines, string filter, bool isParent)
        {
            if (filterLines == null || filterLines.Count == 0)
            {
                return filter;
            }

            if (filterLines.Count == 1)
            {
                if (isParent)
                {
                    filter = GetFilterString(filterLines.First());
                }
                else
                {
                    filter += GetFilterString(filterLines.First());
                }
            }
            else
            {
                var filterComponent = "";
                foreach (var filterLine in filterLines)
                {
                    filterComponent += _andFilterPrefix + GetFilterString(filterLine);
                }
                filterComponent = filterComponent.Remove(0, _andFilterPrefix.Length);
                filter += filterComponent;
            }

            return filter;
        }

        private string GetFilterString(FilterLine filterLine)
        {
            var filterLineValue = GetFilterLineValue(filterLine);
            return $" {filterLine.SelectedField.Name.Camelize()}: {{ {filterLine.SelectedOperator}: {filterLineValue} }}";
        }

        private string BuildParentLevelFilterFields(List<FilterField> filterFields)
        {
            return BuildFilterFields(filterFields, true);
        }

        private string BuildChildLevelFilterFields(List<FilterField> filterFields)
        {
            var childFilterString = new StringBuilder();
            var groupedChildFields = filterFields.Where(f => !string.IsNullOrEmpty(f.ParentFieldName)).GroupBy(f => f.ParentFieldName);

            foreach (var grp in groupedChildFields)
            {
                childFilterString.AppendLine(grp.Key.Camelize());
                childFilterString.AppendLine("{");
                childFilterString.AppendLine(BuildFilterFields(grp.ToList(), false));
                childFilterString.AppendLine("}");
            }

            return childFilterString.ToString();
        }

        private string BuildFilterFields(List<FilterField> filterFields, bool isParentLevel)
        {
            var filterFieldStringBuilder = new StringBuilder();
            if (filterFields != null && filterFields.Any())
            {
                foreach (var field in filterFields.Where(f => f.IsSelected && string.IsNullOrEmpty(f.ParentFieldName) == isParentLevel))
                {
                    filterFieldStringBuilder.AppendLine(field.Name.Camelize());
                };
            }
            return filterFieldStringBuilder.ToString();
        }

        private string GetFilterLineValue(FilterLine filterLine)
        {
            if (ValueIsEnum(filterLine))
            {
                return filterLine.Value.ToString().ToUpper();
            }

            if (ValueRequiresQuotes(filterLine))
            {
                var lineValue = filterLine.Value;
                if(lineValue != null && filterLine.SelectedField.Type.IsDateType())
                {
                    if(DateTime.TryParse(lineValue, out var dateValue))
                    {
                        lineValue = dateValue.ToString("o") + "Z";
                    }
                }
                return $"\"{lineValue}\"";
            }

            return filterLine.Value;
        }

        private bool ValueIsEnum(FilterLine filterLine)
        {
            return filterLine.SelectedField.Type.IsEnum();
        }

        private bool ValueRequiresQuotes(FilterLine filterLine)
        {
            return filterLine.SelectedField.Type.IsStringType() || filterLine.SelectedField.Type.IsDateType();
        }
    }
}
