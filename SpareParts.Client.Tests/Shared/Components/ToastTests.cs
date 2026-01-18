using Microsoft.Extensions.DependencyInjection;
using SpareParts.Client.Shared.Components.Toast;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class ToastTests
    {
        [Fact]
        public void Should_DefaultToNoToastDisplayed()
        {
            var toastService = new ToastService();
            using var ctx = new BunitContext();
            ctx.Services.AddSingleton<IToastService>(toastService);

            var cut = ctx.Render<Toast>();
            var toastElements = cut.FindAll(".toast");
            toastElements.Should().NotBeEmpty();
            toastElements[0].ClassList.Should().Contain("hide");
        }

        [Fact]
        public async Task Should_ShowInfoToastWhenServiceInvoked()
        {
            var toastService = new ToastService();
            using var ctx = new BunitContext();
            ctx.Services.AddSingleton<IToastService>(toastService);
            var cut = ctx.Render<Toast>();

            await cut.InvokeAsync(() =>
            {
                const string message = "This is an info message.";
                toastService.ShowInfo(message);

                var toastElement = cut.Find(".toast");
                toastElement.ClassList.Should().NotContain("hide");
                var toastBody = cut.Find(".toast-body");
                toastBody.TextContent.Should().Contain(message);
                var toastHeader = cut.Find(".toast-header");
                toastHeader.TextContent.Should().Contain(ToastLevel.Info.ToString());
            });            
        }

        [Fact]
        public async Task Should_ShowSuccessToastWhenServiceInvoked()
        {
            var toastService = new ToastService();
            using var ctx = new BunitContext();
            ctx.Services.AddSingleton<IToastService>(toastService);
            var cut = ctx.Render<Toast>();

            await cut.InvokeAsync(() =>
            {
                const string message = "Operation completed successfully.";
                toastService.ShowSuccess(message);

                var toastElement = cut.Find(".toast");
                toastElement.ClassList.Should().NotContain("hide");
                var toastBody = cut.Find(".toast-body");
                toastBody.TextContent.Should().Contain(message);
                var toastHeader = cut.Find(".toast-header");
                toastHeader.TextContent.Should().Contain(ToastLevel.Success.ToString());
            });
        }

        [Fact]
        public async Task Should_ShowWarningToastWhenServiceInvoked()
        {
            var toastService = new ToastService();
            using var ctx = new BunitContext();
            ctx.Services.AddSingleton<IToastService>(toastService);
            var cut = ctx.Render<Toast>();

            await cut.InvokeAsync(() =>
            {
                const string message = "This is a warning message.";
                toastService.ShowWarning(message);

                var toastElement = cut.Find(".toast");
                toastElement.ClassList.Should().NotContain("hide");
                var toastBody = cut.Find(".toast-body");
                toastBody.TextContent.Should().Contain(message);
                var toastHeader = cut.Find(".toast-header");
                toastHeader.TextContent.Should().Contain(ToastLevel.Warning.ToString());
            });
        }

        [Fact]
        public async Task Should_ShowErrorToastWhenServiceInvoked()
        {
            var toastService = new ToastService();
            using var ctx = new BunitContext();
            ctx.Services.AddSingleton<IToastService>(toastService);
            var cut = ctx.Render<Toast>();

            await cut.InvokeAsync(() =>
            {
                const string message = "An error has occurred.";
                toastService.ShowError(message);

                var toastElement = cut.Find(".toast");
                toastElement.ClassList.Should().NotContain("hide");
                var toastBody = cut.Find(".toast-body");
                toastBody.TextContent.Should().Contain(message);
                var toastHeader = cut.Find(".toast-header");
                toastHeader.TextContent.Should().Contain(ToastLevel.Error.ToString());
            });
        }

        [Fact]
        public async Task Should_HideToastWhenServiceHideInvoked()
        {
            var toastService = new ToastService();
            using var ctx = new BunitContext();
            ctx.Services.AddSingleton<IToastService>(toastService);
            var cut = ctx.Render<Toast>();

            await cut.InvokeAsync(() =>
            {
                toastService.ShowInfo("This is an info message.");
                var toastElement = cut.Find(".toast");
                toastElement.ClassList.Should().NotContain("hide");

                toastService.HideToast();

                toastElement.ClassList.Should().Contain("hide");
            });
        }
    }
}
