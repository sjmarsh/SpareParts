using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class TabControlTests
    {
        [Fact]
        public void Should_RenderTabsForSuppliedPages()
        {
            var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.Render<TabControl>(parameters => parameters
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 1"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 2"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 3"))
                );

            var tabs = cut.FindAll(".nav-link");
            tabs.Should().HaveCount(3);
            tabs[0].TextContent.Should().Be("Tab 1");
            tabs[1].TextContent.Should().Be("Tab 2");
            tabs[2].TextContent.Should().Be("Tab 3");
        }

        [Fact]
        public void Should_RenderTabContent()
        {
            const string tabContent = "Hello. I'm inside a tab.";
            var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.Render<TabControl>(parameters => parameters
                .AddChildContent<TabPage>(childParams => childParams
                    .Add(p => p.Text, "Tab 1")
                    .AddChildContent($"<div class='content'>{tabContent}</div>"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 2"))
                );

            var tab = cut.Find(".content");

            tab.InnerHtml.Should().Be(tabContent);
        }

        [Fact]
        public void Should_DefaultToFirstTab()
        {
            var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.Render<TabControl>(parameters => parameters
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 1"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 2"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 3"))
                );

            var tabs = cut.FindAll(".nav-link");
            tabs.Should().HaveCount(3);
            tabs[0].ClassList.Should().Contain("is-selected");
            tabs[1].ClassList.Should().NotContain("is-selected");
            tabs[2].ClassList.Should().NotContain("is-selected");
        }

        [Fact]
        public void Should_SelectTab()
        {
            var ctx = new BunitContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.Render<TabControl>(parameters => parameters
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 1"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 2"))
                .AddChildContent<TabPage>(childParams => childParams.Add(p => p.Text, "Tab 3"))
                );

            var tabs = cut.FindAll(".nav-link");
            tabs.Should().HaveCount(3);
            tabs[0].ClassList.Should().Contain("is-selected");
            tabs[1].ClassList.Should().NotContain("is-selected");

            tabs[1].Click();

            tabs = cut.FindAll(".nav-link");
            tabs.Should().HaveCount(3);
            tabs[0].ClassList.Should().NotContain("is-selected");
            tabs[1].ClassList.Should().Contain("is-selected");
        }
    }
}
