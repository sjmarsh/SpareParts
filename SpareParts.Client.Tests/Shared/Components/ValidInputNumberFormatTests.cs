namespace SpareParts.Client.Tests.Shared.Components
{
    public class ValidInputNumberFormatTests
    {
        [Fact]
        public void Should_RenderValidComponent()
        {
            var testModel = new TestModel();
            var ctx = new BunitContext();
            var cut = ctx.Render<ValidInputNumberFormatWrapper>(parameters => parameters
                .Add(p => p.Id, "testNumber")
                .Add(p => p.DisplayName, "Test Number")
                .Add(p => p.StringFormat, "$")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();

            var label = component.GetElementsByTagName("label")[0];
            label.TextContent.Should().Be("Test Number");
            var input = component.GetElementsByTagName("input")[0];
            input.Id.Should().Be("testNumber");
            input.Attributes.FirstOrDefault(a => a.Name == "type" && a.Value == "string").Should().NotBeNull();
            input.ClassList.Should().Contain("form-control");

            input.Change("$123.00");
            component = cut.Find(".form-group");
            input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().Contain("modified");
            input.ClassList.Should().NotContain("invalid");
            component.InnerHtml.Should().NotContain("validation-message");
        }

        [Fact]
        public void Should_RenderInvalidComponent()
        {
            var testModel = new TestModel();
            var ctx = new BunitContext();
            var cut = ctx.Render<ValidInputNumberFormatWrapper>(parameters => parameters
                .Add(p => p.Id, "testNumber")
                .Add(p => p.DisplayName, "Test Number")
                .Add(p => p.StringFormat, "$")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();
            var input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().NotContain("invalid");

            input.Change("-$1.00");
            component = cut.Find(".form-group");
            input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().Contain("modified");
            input.ClassList.Should().Contain("invalid");
            component.InnerHtml.Should().Contain("validation-message");

        }
    }
}
