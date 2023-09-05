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

            var theFilterLine = new FilterLine(theFields[0], FilterOperator.Equal, "");

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(2);

            var fieldSelect = inputSelects[0] as IHtmlSelectElement;
            fieldSelect.Should().NotBeNull();
            fieldSelect!.Options.Should().HaveCount(2);
            fieldSelect!.Options.Select(o => o.GetInnerText()).Should().BeEquivalentTo(new[] { "Field1", "Field2" });
        }

        [Fact]
        public void Should_RenderOperatorsForTextValue()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            var theFilterLine = new FilterLine(theFields[0], FilterOperator.Equal, "");

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(2);

            var operatorSelect = inputSelects[1] as IHtmlSelectElement;
            operatorSelect.Should().NotBeNull();
            operatorSelect!.Options.Should().HaveCount(5);
            operatorSelect!.Options.Select(o => o.Value).Should().BeEquivalentTo(FilterOperator.NamedFilterOperatorsForStrings().Select(o => o.Name).ToList());
        }

        [Fact]
        public void Should_RenderOperatorsForNumericValue()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            var theFilterLine = new FilterLine(theFields[1], FilterOperator.Equal, "0");

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(2);

            var operatorSelect = inputSelects[1] as IHtmlSelectElement;
            operatorSelect.Should().NotBeNull();
            operatorSelect!.Options.Should().HaveCount(6);
            operatorSelect!.Options.Select(o => o.Value).Should().BeEquivalentTo(FilterOperator.NamedFilterOperatorsForDatesAndNumbers().Select(o => o.Name).ToList());
        }

        [Fact]
        public void Should_RenderFilterValue()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            const string TheValue = "The Value";
            var theFilterLine = new FilterLine(theFields[0], FilterOperator.Equal, TheValue);

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputValues = cut.FindAll(".form-control");
            inputValues.Should().HaveCount(1);
            var inputValue = inputValues[0] as IHtmlInputElement;
            inputValue!.Value.Should().Be(TheValue);
        }

        [Fact]
        public void Should_RenderFilterValueOptionsWhenFieldIsAnEnum()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(TestEnum), true)
            };

            var theFilterLine = new FilterLine(theFields[0], FilterOperator.Equal, TestEnum.None.ToString());

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputSelects = cut.FindAll(".form-select");

            inputSelects.Should().NotBeNull();
            inputSelects.Should().HaveCount(3);

            var operatorSelect = inputSelects[1] as IHtmlSelectElement;
            operatorSelect.Should().NotBeNull();
            operatorSelect!.Options.Should().HaveCount(5);
            operatorSelect!.Options.Select(o => o.Value).Should().BeEquivalentTo(FilterOperator.NamedFilterOperatorsForStrings().Select(o => o.Name).ToList());

            var valueSelect = inputSelects[2] as IHtmlSelectElement;
            valueSelect.Should().NotBeNull();
            valueSelect!.Options.Should().HaveCount(4);
            valueSelect!.Options.Select(o => o.Value).Should().BeEquivalentTo(new[] {"None", "One", "Two", "Three"} );
        }

        [Fact]
        public void Should_UpdateFilterLine()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };

            const string TheValue = "The Value";
            var theFilterLine = new FilterLine(theFields[0], FilterOperator.Equal, TheValue);

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
            );

            var inputValue = cut.Find("#value");
            inputValue.Input("Test");

            theFilterLine.Value.Should().Be("Test");
        }

        [Fact]
        public void Should_HandleRemoveFilter()
        {
            using var ctx = new TestContext();
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true)
            };
            bool isRemovedCalled = false;
                        
            var theFilterLine = new FilterLine(theFields[0], FilterOperator.Equal, "");

            var cut = ctx.RenderComponent<FilterSelectorWrapper>(parameters =>
                parameters.Add(p => p.Fields, theFields)
                          .Add(p => p.FilterLine, theFilterLine)
                          .Add(p => p.OnRemoveFilter, () => { isRemovedCalled = true; })
            );

            var removeButton = cut.Find("#remove");
            removeButton.Should().NotBeNull();
            removeButton.Click();

            isRemovedCalled.Should().BeTrue();
        }
    }

    public enum TestEnum
    {
        None,
        One,
        Two,
        Three
    }
}
