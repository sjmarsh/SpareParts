using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class IconButtonTests
    {
        [Fact]
        public void Should_RenderButtonTitle()
        {
            const string TheTitle = "Click Me";
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<IconButton>(parameters => parameters
                .Add(p => p.ButtonTitle, TheTitle)
                .Add(p => p.Icon, IconButtonIcon.Print));

            var button = cut.Find("button");

            button.Should().NotBeNull();
            button.TextContent.Should().Be(TheTitle); 
        }

        [Fact]
        public void Should_NotRenderButtonTitleIfHidden()
        {
            const string TheTitle = "Click Me";
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<IconButton>(parameters => parameters
                .Add(p => p.ButtonTitle, TheTitle)
                .Add(p => p.IsTitleVisible, false)
                .Add(p => p.Icon, IconButtonIcon.Print));

            var button = cut.Find("button");

            button.Should().NotBeNull();
            button.TextContent.Should().BeEmpty();
        }

        [Fact]
        public void Should_RenderIcon()
        {
            const string TheTitle = "Click Me";
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<IconButton>(parameters => parameters
                .Add(p => p.ButtonTitle, TheTitle)
                .Add(p => p.IsTitleVisible, false)
                .Add(p => p.Icon, IconButtonIcon.Print));

            var icon = cut.FindAll("span").FirstOrDefault();
            icon.Should().NotBeNull();
            icon.ClassName.Should().Contain(IconButtonIcon.Print.GetCss());
        }
    }
}
