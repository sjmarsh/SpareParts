using Microsoft.Extensions.DependencyInjection;
using SpareParts.Client.Services;
using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class MessageBoxTests
    {
        [Fact]
        public async Task Should_ShowMessageBoxWithYesNo()
        {
            var messageBoxService = new MessageBoxService();
            string msg = "The Message";
            string title = "The Title";
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async() => {
                
                var messageTask = messageBoxService.ShowMessage(msg, title, MessageBoxType.YesNo);
               
                var renderedMessage = cut.Find(".message");
                renderedMessage.Should().NotBeNull();
                var renderedTitle = cut.Find(".modal-title");
                title.Should().NotBeNull();
                var yesButton = cut.Find(".btn-primary");
                yesButton.TextContent.Should().Be("Yes");
                var noButton = cut.Find(".btn-secondary");
                noButton.TextContent.Should().Be("No");
                yesButton.Click();

                await messageTask;
            });
        }

        [Fact]
        public async Task Should_ShowMessageBoxWithOkCancel()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.OKCancel);
                                
                var okButton = cut.Find(".btn-primary");
                okButton.TextContent.Should().Be("OK");
                var cancelButton = cut.Find(".btn-secondary");
                cancelButton.TextContent.Should().Be("Cancel");
                okButton.Click();

                await messageTask;
            });
        }

        [Fact]
        public async Task Should_ShowMessageBoxWithOKButton()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.OK);

                var okButton = cut.Find(".btn-primary");
                okButton.TextContent.Should().Be("OK");
                var cancelButtons = cut.FindAll(".btn-secondary");
                cancelButtons.Should().HaveCount(0);
                okButton.Click();

                await messageTask;
            });
        }

        [Fact]
        public async Task Should_ReturnYesResult()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.YesNo);
                                
                var yesButton = cut.Find(".btn-primary");
                yesButton.TextContent.Should().Be("Yes");
                yesButton.Click();

                var result = await messageTask;
                result.Should().Be(MessageBoxResult.Yes);
            });
        }

        [Fact]
        public async Task Should_ReturnNoResult()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.YesNo);

                var noButton = cut.Find(".btn-secondary");
                noButton.TextContent.Should().Be("No");
                noButton.Click();

                var result = await messageTask;
                result.Should().Be(MessageBoxResult.No);
            });
        }

        [Fact]
        public async Task Should_ReturnOkResult()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.OKCancel);

                var okButton = cut.Find(".btn-primary");
                okButton.TextContent.Should().Be("OK");
                okButton.Click();

                var result = await messageTask;
                result.Should().Be(MessageBoxResult.OK);
            });
        }

        [Fact]
        public async Task Should_ReturnCancelResult()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.OKCancel);

                var cancelButton = cut.Find(".btn-secondary");
                cancelButton.TextContent.Should().Be("Cancel");
                cancelButton.Click();

                var result = await messageTask;
                result.Should().Be(MessageBoxResult.Cancel);
            });
        }

        [Fact]
        public async Task Should_ReturnOkResultWhenOKType()
        {
            var messageBoxService = new MessageBoxService();
            using var ctx = new TestContext();
            ctx.Services.AddSingleton<IMessageBoxService>(messageBoxService);
            var cut = ctx.RenderComponent<MessageBox>();

            var renderedMessages = cut.FindAll(".message");
            renderedMessages.Should().HaveCount(0);

            await cut.InvokeAsync(async () => {

                var messageTask = messageBoxService.ShowMessage("message", "title", MessageBoxType.OK);

                var okButton = cut.Find(".btn-primary");
                okButton.TextContent.Should().Be("OK");
                okButton.Click();

                var result = await messageTask;
                result.Should().Be(MessageBoxResult.OK);
            });
        }
    }
}
