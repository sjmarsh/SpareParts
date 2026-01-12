using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class ModalTests
    {
        [Fact]
        public void Should_ShowModal()
        {
            using var ctx = new TestContext();
            var cut = ctx.Render<Modal>(parameters => parameters.Add(p => p.ShowModal, true));

            var modal = cut.Find(".modal");
            modal.Should().NotBeNull();
        }

        [Fact]
        public void Should_HideModal()
        {
            using var ctx = new TestContext();
            var cut = ctx.Render<Modal>(parameters => parameters.Add(p => p.ShowModal, false));

            cut.Markup.Should().BeEmpty();
        }

        [Fact]
        public void Should_RenderHeaderText()
        {
            const string header = "This is the Header";
            using var ctx = new TestContext();
            var cut = ctx.Render<Modal>(parameters => parameters
                .Add(p => p.ShowModal, true)
                .Add(p => p.HeaderText, header));

            var actualHeader = cut.Find(".modal-title").TextContent;
            actualHeader.Should().Be(header);
        }

        [Fact]
        public void Should_RenderCloseButtonText()
        {
            const string closeText = "Close Me";
            using var ctx = new TestContext();
            var cut = ctx.Render<Modal>(parameters => parameters
                .Add(p => p.ShowModal, true)
                .Add(p => p.CloseButtonText, closeText));

            var actualCloseText = cut.Find("#closeModal").TextContent;
            actualCloseText.Should().Be(closeText);
        }

        [Fact]
        public void Should_RenderModalContent()
        {
            const string modalContent = "<div>This is my modal content!</div>";
            using var ctx = new TestContext();
            var cut = ctx.Render<Modal>(parameters => parameters
                .Add(p => p.ShowModal, true)
                .AddChildContent(modalContent));

            cut.Markup.Should().Contain(modalContent);
        }

        [Fact]
        public void CloseButtonClick_Should_HideModal()
        {
            using var ctx = new TestContext();
            var cut = ctx.Render<Modal>(parameters => parameters
                .Add(p => p.ShowModal, true));

            cut.Markup.Should().NotBeEmpty();

            var closeButton = cut.Find("#closeModal");
            closeButton.Click();

            cut.Markup.Should().BeEmpty();
        }
    }
}
