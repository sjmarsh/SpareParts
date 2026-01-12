namespace SpareParts.Client.Tests.Shared.Components
{
    public class ValidInputNumberTests
    {
        [Fact]
        public void Should_RenderValidComponent()
        {
            var testModel = new TestModel();
            var ctx = new TestContext();
            var cut = ctx.Render<ValidInputNumberWrapper>(parameters => parameters
                .Add(p => p.Id, "testNumber")
                .Add(p => p.DisplayName, "Test Number")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();

            var label = component.GetElementsByTagName("label")[0];
            label.TextContent.Should().Be("Test Number");
            var input = component.GetElementsByTagName("input")[0];
            input.Id.Should().Be("testNumber");
            input.Attributes.FirstOrDefault(a => a.Name == "type" && a.Value == "number").Should().NotBeNull();
            input.ClassList.Should().Contain("valid");
            input.ClassList.Should().Contain("form-control");

            input.Input("123");
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
            var cut = ctx.Render<ValidInputNumberWrapper>(parameters => parameters
                .Add(p => p.Id, "testNumber")
                .Add(p => p.DisplayName, "Test Number")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();            
            var input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().Contain("valid");
            
            input.Input("-1");
            component = cut.Find(".form-group");
            input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().Contain("invalid");
            component.InnerHtml.Should().Contain("validation-message");
        }
    }
}
