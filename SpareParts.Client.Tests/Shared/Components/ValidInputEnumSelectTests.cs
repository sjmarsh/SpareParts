using AngleSharp.Html.Dom;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class ValidInputEnumSelectTests
    {
        [Fact]
        public void Should_RenderValidComponent()
        {
            var testModel = new TestModel();
            var ctx = new BunitContext();
            var cut = ctx.Render<ValidInputEnumSelectWrapper>(parameters => parameters
                .Add(p => p.Id, "testSelect")
                .Add(p => p.DisplayName, "Test Select")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();

            var label = component.GetElementsByTagName("label")[0];
            label.TextContent.Should().Be("Test Select");
            var select = component.GetElementsByTagName("select")[0] as IHtmlSelectElement;
            select.Should().NotBeNull();
            select!.Id.Should().Be("testSelect");
            select.ClassList.Should().Contain("valid");
            select.ClassList.Should().Contain("form-select");


            select.Change(TestClientEnum.One.ToString());
            component = cut.Find(".form-group");
            select = component.GetElementsByTagName("select")[0] as IHtmlSelectElement;
            select.Should().NotBeNull();
            select!.ClassList.Should().NotContain("invalid");
            component.InnerHtml.Should().NotContain("validation-message");
        }
    }
}
