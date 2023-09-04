using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class TooltipTests
    {
        [Fact]
        public void Should_RenderTooltip()
        {
            const string TooltipContent = "The Tooltip";
            string inputTextVal = "Test";
            Expression<Func<string>> inputTextExp = () => inputTextVal;
            var ctx = new TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            
            var cut = ctx.RenderComponent<Tooltip>(parameters => parameters
                .Add(t => t.TooltipContent, TooltipContent)
                .AddChildContent<InputText>(t => t.Add(i => i.Value, inputTextVal).Add(i => i.ValueExpression, inputTextExp)));

            var tooltip = cut.Find(".blazor-tooltip");
            tooltip.Should().NotBeNull();   
        }

        [Fact]
        public void Should_NotRenderTooltipWhenNotProvided()
        {
            const string? TooltipContent = null;
            string inputTextVal = "Test";
            Expression<Func<string>> inputTextExp = () => inputTextVal;
            var ctx = new TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            var cut = ctx.RenderComponent<Tooltip>(parameters => parameters
                .Add(t => t.TooltipContent, TooltipContent)
                .AddChildContent<InputText>(t => t.Add(i => i.Value, inputTextVal).Add(i => i.ValueExpression, inputTextExp)));

            var tooltips = cut.FindAll(".blazor-tooltip");
            tooltips.Should().HaveCount(0);
        }
    }
}
