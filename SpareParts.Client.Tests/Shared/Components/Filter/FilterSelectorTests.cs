using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Tests.Shared.Components.Filter
{
    public class FilterSelectorTests
    {
        [Fact]
        public void Should_RenderSelectedFilterFields()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() { 
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true),
                new FilterField("Field3", typeof(DateTime), false) // not selected so should not render
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            var theFilterLine = new FilterLine(theFields[0], theOperators[0].FilterOperator, "");

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.Operators, theOperators)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(2);

            var fieldSelect = inputSelects[0] as IHtmlSelectElement;
            fieldSelect.Should().NotBeNull();
            fieldSelect.Options.Should().HaveCount(2);
            fieldSelect.Options.Select(o => o.GetInnerText()).Should().BeEquivalentTo(new[] { "Field1", "Field2" });
        }

        [Fact]
        public void Should_RenderOperators()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            var theFilterLine = new FilterLine(theFields[0], theOperators[0].FilterOperator, "");

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.Operators, theOperators)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(2);

            var operatorSelect = inputSelects[1] as IHtmlSelectElement;
            operatorSelect.Should().NotBeNull();
            operatorSelect.Options.Should().HaveCount(2);
            operatorSelect.Options.Select(o => o.GetInnerText()).Should().BeEquivalentTo(new[] { "Equals", "Contains" });
        }

        [Fact]
        public void Should_RenderFilterValue()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            const string TheValue = "The Value";
            var theFilterLine = new FilterLine(theFields[0], theOperators[0].FilterOperator, TheValue);

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.Operators, theOperators)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputValue = cut.Find("#value");
            inputValue.GetInnerText().Should().Be(TheValue);
        }

        [Fact]
        public void Should_UpdateFilterLine()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            const string TheValue = "The Value";
            var theFilterLine = new FilterLine(theFields[0], theOperators[0].FilterOperator, TheValue);

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.Operators, theOperators)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputValue = cut.Find("#value");
            inputValue.Input("Test");

            theFilterLine.Value.Should().Be("Test");
        }

    }
}
