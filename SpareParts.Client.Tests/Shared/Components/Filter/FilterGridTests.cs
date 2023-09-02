using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Microsoft.Extensions.DependencyInjection;
using SpareParts.Shared.Models;
using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Tests.Shared.Components.Filter
{
    public class FilterGridTests
    {
        [Fact]
        public void Should_ShowFieldChips()
        {
            using var ctx = new TestContext();
            const string fakeQuery = "the built query";
            var fakeGraphQLBuilder = new FakeGraphQLRequestBuilder(fakeQuery);
            ctx.Services.AddSingleton<IGraphQLRequestBuilder>(fakeGraphQLBuilder);
            var cut = ctx.RenderComponent<FilterGrid<ThingToFilter>>();

            var chips = cut.FindAll(".badge");
            chips.Should().NotBeNull();
            chips.Should().HaveCount(3);  // should ignore ID
            chips[0].GetInnerText().Should().Be("Name X");
            chips[1].GetInnerText().Should().Be("Value X");
            chips[2].GetInnerText().Should().Be("DateAdded X");
        }

        [Fact]
        public void Should_HaveDefaultFilterWithFirstField() 
        {
            using var ctx = new TestContext();
            const string fakeQuery = "the built query";
            var fakeGraphQLBuilder = new FakeGraphQLRequestBuilder(fakeQuery);
            ctx.Services.AddSingleton<IGraphQLRequestBuilder>(fakeGraphQLBuilder);
            var cut = ctx.RenderComponent<FilterGrid<ThingToFilter>>();

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(2);
            inputSelects[0].Children.Should().HaveCount(3);
            var fieldSelect = inputSelects[0] as IHtmlSelectElement;
            fieldSelect!.Value.Should().Be("Name");
            var operatorSelect = inputSelects[1] as IHtmlSelectElement;
            operatorSelect!.Value.Should().Be(FilterOperator.Equal);

            var inputText = cut.Find("#value");
            inputText.GetInnerText().Should().Be("");
        }

        [Fact]
        public void Should_AddEmptyFilterWhenAddClicked()
        {
            using var ctx = new TestContext();
            const string fakeQuery = "the built query";
            var fakeGraphQLBuilder = new FakeGraphQLRequestBuilder(fakeQuery);
            ctx.Services.AddSingleton<IGraphQLRequestBuilder>(fakeGraphQLBuilder);
            var cut = ctx.RenderComponent<FilterGrid<ThingToFilter>>();

            var buttons = cut.FindAll("button");
            var addFilterButton = buttons.Single(b => b.GetInnerText() == "Add Filter");
            addFilterButton.Click();

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(4);

            var inputTexts = cut.FindAll("#value");
            inputTexts.Select(i => i.GetInnerText()).Should().BeEquivalentTo(new[] { "", "" });
        }

        [Fact]
        public void Should_CallSearchWithValidFilterSelections()
        {
            using var ctx = new TestContext();
            const string fakeQuery = "the built query";
            var fakeGraphQLBuilder = new FakeGraphQLRequestBuilder(fakeQuery);
            ctx.Services.AddSingleton<IGraphQLRequestBuilder>(fakeGraphQLBuilder);
            GraphQLRequest? theRequest = null;
            Func<GraphQLRequest, Task<List<ThingToFilter>>> theServiceCall = (request) => { 
                theRequest = request;
                return Task.FromResult(new List<ThingToFilter>());
            };
            var cut = ctx.RenderComponent<FilterGrid<ThingToFilter>>(parameters => parameters.Add(p => p.ServiceCall, theServiceCall));

            var inputSelects = cut.FindAll(".form-select");
            var fieldSelect = inputSelects[0];
            fieldSelect.Change("Value");
            inputSelects = cut.FindAll(".form-select");
            var operatorSelect = inputSelects[1];
            operatorSelect!.Change(FilterOperator.GreaterThan);
            var inputText = cut.Find("#value");
            inputText.Input("2");

            var buttons = cut.FindAll("button");
            var search = buttons.Single(b => b.GetInnerText() == "Search");
            search.Click();

            theRequest.Should().NotBeNull();
            theRequest!.query.Should().Be(fakeQuery);
            fakeGraphQLBuilder.FilterFields.Should().HaveCount(3);
            fakeGraphQLBuilder.FilterFields!.Count(f => f.IsSelected).Should().Be(3);
            fakeGraphQLBuilder.FilterFields!.Select(f => f.Name).Should().BeEquivalentTo(new[] { "Name", "Value", "DateAdded" });
            
            fakeGraphQLBuilder.FilterLines.Should().HaveCount(1);
            fakeGraphQLBuilder.FilterLines![0].SelectedField.Name.Should().Be("Value");
            fakeGraphQLBuilder.FilterLines![0].SelectedOperator.Should().Be(FilterOperator.GreaterThan);
            fakeGraphQLBuilder.FilterLines![0].Value.Should().Be("2");
        }

        [Fact]
        public void Should_NotCallSearchWhenInvalidFilterSelection()
        {
            using var ctx = new TestContext();
            const string fakeQuery = "the built query";
            var fakeGraphQLBuilder = new FakeGraphQLRequestBuilder(fakeQuery);
            ctx.Services.AddSingleton<IGraphQLRequestBuilder>(fakeGraphQLBuilder);
            GraphQLRequest? theRequest = null;
            Func<GraphQLRequest, Task<List<ThingToFilter>>> theServiceCall = (request) => {
                theRequest = request;
                return Task.FromResult(new List<ThingToFilter>());
            };
            var cut = ctx.RenderComponent<FilterGrid<ThingToFilter>>(parameters => parameters.Add(p => p.ServiceCall, theServiceCall));

            var inputSelects = cut.FindAll(".form-select");
            var fieldSelect = inputSelects[0];
            fieldSelect.Change("Value");
            inputSelects = cut.FindAll(".form-select");
            var operatorSelect = inputSelects[1];
            operatorSelect!.Change(FilterOperator.GreaterThan);
            var inputText = cut.Find("#value");
            inputText.Input(""); // should make this invalid

            var buttons = cut.FindAll("button");
            var search = buttons.Single(b => b.GetInnerText() == "Search");
            search.Click();

            theRequest.Should().BeNull();

            var errorMessage = cut.Find(".alert");
            errorMessage.GetInnerText().Should().Contain("invalid");
        }
    }

    public class ThingToFilter
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public double Value { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class FakeGraphQLRequestBuilder : IGraphQLRequestBuilder
    {
        private string? _queryToReturn;

        public FakeGraphQLRequestBuilder(string queryToReturn)
        {
            _queryToReturn = queryToReturn;
        }

        public List<FilterLine>? FilterLines { get; set; }
        public List<FilterField>? FilterFields { get; set; }
        public string? RootGraphQLField { get; set; }
        public PageOffset? PageOffset { get; set; }

        public GraphQLRequest Build<T>(List<FilterLine> filterLines, List<FilterField> filterFields, string? rootGraphQLField = null, PageOffset? pageOffset = null)
        {
            FilterLines = filterLines;
            FilterFields = filterFields;
            RootGraphQLField = rootGraphQLField;
            PageOffset = pageOffset;

            return new GraphQLRequest { query = _queryToReturn };
        }
    }
}
