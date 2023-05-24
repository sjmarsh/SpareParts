using Humanizer;
using SpareParts.Client.Services;
using System.Text;

namespace SpareParts.Client.Shared.Components.Filter
{
    public interface IGraphQLRequestBuilder
    {
        GraphQLRequest Build<T>(List<FilterLine> filterLines, List<FilterField> filterFields, string? rootGraphQLField = null);
    }

    public class GraphQLRequestBuilder : IGraphQLRequestBuilder
    {
        public GraphQLRequest Build<T>(List<FilterLine> filterLines, List<FilterField> filterFields, string? rootGraphQLField = null)
        {
            var filter = BuildQueryFilter(filterLines);
            var fields = BuildFilterFields(filterFields);
            var rootField = rootGraphQLField ?? $"{typeof(T).Name.ToLower()}s";

            var graphQLRequest = new GraphQLRequest
            {
                query = $@"{{
                        {rootField} (where: {{{filter}}}) {{
                            {fields}
                        }}
                    }}"
            };

            return graphQLRequest;
        }

        private string BuildQueryFilter(List<FilterLine> filterLines)
        {
            var filter = "";
            if (filterLines != null)
            {
                if (filterLines.Count == 1)
                {
                    filter = GetFilterString(filterLines.First());
                }
                else
                {
                    const string filterAndPrefix = " and: {";

                    foreach (var filterLine in filterLines)
                    {
                        filter += filterAndPrefix + GetFilterString(filterLine);
                    }
                    filter = filter.Remove(0, filterAndPrefix.Length);
                }

                for (int i = 0; i < filterLines.Count - 1; i++)
                {
                    filter += "}";
                }
            }
            return filter;
        }

        private string BuildFilterFields(List<FilterField> filterFields)
        {
            var filterFieldStringBuilder = new StringBuilder();
            if (filterFields != null && filterFields.Any())
            {
                foreach (var field in filterFields.Where(f => f.IsSelected))
                {
                    filterFieldStringBuilder.AppendLine(field.Name.Camelize());
                };
            }
            return filterFieldStringBuilder.ToString();
        }

        private string GetFilterString(FilterLine filterLine)
        {
            var filterLineValue = GetFilterLineValue(filterLine);
            return $" {filterLine.SelectedField.Name.Camelize()}: {{{filterLine.SelectedOperator}:{filterLineValue} }}";
        }

        private string GetFilterLineValue(FilterLine filterLine)
        {
            if (ValueRequiresQuotes(filterLine))
            {
                return $"\"{filterLine.Value}\"";
            }

            return filterLine.Value;
        }

        private bool ValueRequiresQuotes(FilterLine filterLine)
        {
            return filterLine.SelectedField.Type.IsAssignableFrom(typeof(string)) ||
                    filterLine.SelectedField.Type.IsAssignableFrom(typeof(DateTime)) ||
                    filterLine.SelectedField.Type.IsAssignableFrom(typeof(DateTime?));
        }
    }
}
