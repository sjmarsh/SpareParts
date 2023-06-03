using AngleSharp.Dom;
using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class ChipsListTests
    {
        [Fact]
        public void Should_RenderChips()
        {
            List<Chip> theChips = new()
            {
                new Chip("Chip1", true),
                new Chip("Chip2", true)
            };

            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips));

            var chipList = cut.FindAll(".badge");
            chipList.Should().NotBeNullOrEmpty();
            chipList.Should().HaveCount(2);
            var chipNames = chipList.Select(c => c.GetInnerText());
            chipNames.Should().NotBeNullOrEmpty();
            chipNames.Should().BeEquivalentTo(theChips.Select(c => c.Name + " X").ToList());
            chipList[0].ClassList.Should().Contain("bg-dark");
            chipList[1].ClassList.Should().Contain("bg-dark");
        }

        [Fact]
        public void Should_RenderInactiveChips()
        {
            List<Chip> theChips = new()
            {
                new Chip("Chip1", true),
                new Chip("Chip2", false) // inactive
            };

            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips));

            var chipList = cut.FindAll(".badge");
            chipList.Should().NotBeNullOrEmpty();
            chipList.Should().HaveCount(2);
            chipList[0].ClassList.Should().Contain("bg-dark");
            chipList[1].ClassList.Should().Contain("bg-light"); // inactive chip should be light
        }

        [Fact]
        public void Should_ToggleActiveChip()
        {
            List<Chip> theChips = new()
            {
                new Chip("Chip1", true),
                new Chip("Chip2", true)
            };
            var wasToggled = false;

            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips)
                            .Add(p => p.OnToggleChip, () => { wasToggled = true; }));

            var chipList = cut.FindAll(".badge");
            chipList[0].ClassList.Should().Contain("bg-dark");

            var chipButtons = cut.FindAll(".chips-custom");
            chipButtons.Should().NotBeNullOrEmpty();
            chipButtons.Should().HaveCount(2);

            chipButtons[0].Children[0].Click(); // should toggle to inactive (with light class)

            chipList = cut.FindAll(".badge");
            chipList[0].ClassList.Should().Contain("bg-light");

            wasToggled.Should().BeTrue();
        }

        [Fact]
        public void Should_SetTitle()
        {
            List<Chip> theChips = new()
            {
                new Chip("Chip1", true),
                new Chip("Chip2", true)
            };

            const string TheTitle = "The Title";

            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<ChipsList>(parameters => parameters
                            .Add(p => p.Chips, theChips)
                            .Add(p => p.Title, TheTitle));

            var title = cut.Find(".card-subtitle");
            title.GetInnerText().Should().Be(TheTitle);
        }
    }
}
