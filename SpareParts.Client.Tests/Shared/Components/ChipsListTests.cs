using AngleSharp.Dom;
using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class ChipsListTests
    {
        private const string ChipClass = ".chip";

        [Fact]
        public void Should_RenderChips()
        {
            List<Chip> theChips = new()
            {
                new Chip(Guid.NewGuid(), "Chip1", true),
                new Chip(Guid.NewGuid(), "Chip2", true)
            };

            using var ctx = new TestContext();
            var cut = ctx.Render<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips));

            var chipList = cut.FindAll(ChipClass);
            chipList.Should().NotBeNullOrEmpty();
            chipList.Should().HaveCount(2);
            var chipNames = chipList.Select(c => c.GetInnerText());
            chipNames.Should().NotBeNullOrEmpty();
            chipNames.Should().BeEquivalentTo(theChips.Select(c => c.Name).ToList());
            chipList[0].ClassList.Should().Contain("chip-color-default");
            chipList[1].ClassList.Should().Contain("chip-color-default");
        }

        [Fact]
        public void Should_RenderInactiveChips()
        {
            List<Chip> theChips = new()
            {
                new Chip(Guid.NewGuid(), "Chip1", true),
                new Chip(Guid.NewGuid(), "Chip2", false) // inactive
            };

            using var ctx = new TestContext();
            var cut = ctx.Render<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips));

            var chipList = cut.FindAll(ChipClass);
            chipList.Should().NotBeNullOrEmpty();
            chipList.Should().HaveCount(2);
            chipList[0].ClassList.Should().Contain("chip-color-default");
            chipList[1].ClassList.Should().Contain("chip-color-default-outlined"); // inactive chip should be outlined
        }

        [Fact]
        public void Should_ToggleActiveChip()
        {
            List<Chip> theChips = new()
            {
                new Chip(Guid.NewGuid(), "Chip1", true),
                new Chip(Guid.NewGuid(), "Chip2", true)
            };
            var wasToggled = false;

            using var ctx = new TestContext();
            var cut = ctx.Render<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips)
                            .Add(p => p.OnToggleChip, () => { wasToggled = true; }));

            var chipList = cut.FindAll(ChipClass);
            chipList[0].ClassList.Should().Contain("chip-color-default");

            var chipButtons = cut.FindAll(".chip-icon");
            chipButtons.Should().NotBeNullOrEmpty();
            chipButtons.Should().HaveCount(2);

            chipButtons[0].Children[0].Click(); // should toggle to inactive (with light class)

            chipList = cut.FindAll(ChipClass);
            chipList[0].ClassList.Should().Contain("chip-color-default-outlined");

            wasToggled.Should().BeTrue();
        }

        [Fact]
        public void Should_SetTitle()
        {
            List<Chip> theChips = new()
            {
                new Chip(Guid.NewGuid(), "Chip1", true),
                new Chip(Guid.NewGuid(), "Chip2", true)
            };

            const string TheTitle = "The Title";

            using var ctx = new TestContext();
            var cut = ctx.Render<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips)
                            .Add(p => p.Title, TheTitle));

            var title = cut.Find(".card-subtitle");
            title.GetInnerText().Should().Be(TheTitle);
        }
    }
}
