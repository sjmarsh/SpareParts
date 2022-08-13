namespace SpareParts.Client.Tests.Shared.Components
{
    public class ValidInputTextTests
    {
        [Fact]
        public void Should_RenderValidComponent()
        {
            var testModel = new TestModel();
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<ValidInputTextWrapper>(parameters => parameters
                .Add(p => p.Id, "testText")
                .Add(p => p.DisplayName, "Test Text")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();

            var label = component.GetElementsByTagName("label")[0];
            label.TextContent.Should().Be("Test Text");
            var input = component.GetElementsByTagName("input")[0];
            input.Id.Should().Be("testText");
            input.Attributes.FirstOrDefault(a => a.Name == "type" && a.Value == "text").Should().NotBeNull();
            input.ClassList.Should().Contain("valid");
            input.ClassList.Should().Contain("form-control");

            input.Input("abcd");
            component = cut.Find(".form-group");
            input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().NotContain("invalid");
            component.InnerHtml.Should().NotContain("validation-message");
        }

        [Fact]
        public void Should_RenderInvalidComponent()
        {
            var testModel = new TestModel();
            var ctx = new TestContext();
            var cut = ctx.RenderComponent<ValidInputTextWrapper>(parameters => parameters
                .Add(p => p.Id, "testText")
                .Add(p => p.DisplayName, "Test Text")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();
            var input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().Contain("valid");

            input.Input("a string larger than ten characters");
            component = cut.Find(".form-group");
            input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().Contain("invalid");
            component.InnerHtml.Should().Contain("validation-message");
        }
    }
}
