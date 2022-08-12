using Microsoft.AspNetCore.Components.Web;
using SpareParts.Client.Shared.Components;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class PagerTests
    {
        // default page size is 10

        [Fact]
        public void Should_DisplaySinglePageNumberWhenTotalItemCountLessThanDefaultPageSize()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 5));

            var pageItems = cut.FindAll(".page-item");
            pageItems.Should().HaveCount(3); // | << | 1 | >> |

            pageItems[1].FirstChild!.TextContent.Should().Be("1");
        }

        [Fact]
        public void Should_DisplaySinglePageNumberWhenTotalItemCountLessThanSuppliedPageSize()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters
                .Add(p => p.TotalItemCount, 5)
                .Add(p => p.PageSize, 6));

            var pageItems = cut.FindAll(".page-item");
            pageItems.Should().HaveCount(3); // | << | 1 | >> |

            pageItems[1].FirstChild!.TextContent.Should().Be("1");
        }

        [Fact]
        public void Should_DisplayPageNumbersWhenTotalItemCountMoreThanDefaultPageSize()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 11));

            var pageItems = cut.FindAll(".page-item");
            pageItems.Should().HaveCount(4); // | << | 1 | 2 | >> |

            pageItems[1].FirstChild!.TextContent.Should().Be("1");
            pageItems[2].FirstChild!.TextContent.Should().Be("2");
        }

        [Fact]
        public void Should_DisplayPageNumbersWhenTotalItemCountMoreThanSuppliedPageSize()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters
                .Add(p => p.TotalItemCount, 11)
                .Add(p => p.PageSize, 7));

            var pageItems = cut.FindAll(".page-item");
            pageItems.Should().HaveCount(4); // | << | 1 | 2 | >> |

            pageItems[1].FirstChild!.TextContent.Should().Be("1");
            pageItems[2].FirstChild!.TextContent.Should().Be("2");
        }

        [Fact]
        public void Should_DefaultFirstPageToActive()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 11));

            var pageItems = cut.FindAll(".page-item");

            pageItems[1].ClassList.Should().Contain("active");
            pageItems[2].ClassList.Should().NotContain("active");
        }

        [Fact]
        public void Should_SetActivePageWhenSelectedPageIsDifferentToCurrent()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 11));
                        
            var pageItems = cut.FindAll(".page-item");
            pageItems[2].FirstElementChild!.Click();
            
            pageItems = cut.FindAll(".page-item");
            pageItems[1].ClassList.Should().NotContain("active");
            pageItems[2].ClassList.Should().Contain("active");
        }

        [Fact]
        public void Should_SetActivePageWhenSelectedPageIsSameAsCurrent()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 11));

            var pageItems = cut.FindAll(".page-item");
            pageItems[2].FirstElementChild!.Click();

            pageItems = cut.FindAll(".page-item");
            pageItems[1].ClassList.Should().NotContain("active");
            pageItems[2].ClassList.Should().Contain("active");

            pageItems[2].FirstElementChild!.Click();

            pageItems = cut.FindAll(".page-item");
            pageItems[1].ClassList.Should().NotContain("active");
            pageItems[2].ClassList.Should().Contain("active");
        }

        [Fact]
        public void Should_SetActivePageToFirstPage()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 11));

            var pageItems = cut.FindAll(".page-item");
            pageItems[2].FirstElementChild!.Click(); // go to page 2
            
            pageItems = cut.FindAll(".page-item");
            pageItems[1].ClassList.Should().NotContain("active");  

            pageItems[0].FirstElementChild!.Id.Should().Be("FirstPage"); // click the first page button
            pageItems[0].FirstElementChild!.Click();

            pageItems = cut.FindAll(".page-item");
            pageItems[1].ClassList.Should().Contain("active");   
        }

        [Fact]
        public void Should_SetActivePageToLastPage()
        {
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters.Add(p => p.TotalItemCount, 11));

            var pageItems = cut.FindAll(".page-item");
            pageItems[0].FirstElementChild!.Click(); // go to page 1

            pageItems = cut.FindAll(".page-item");
            pageItems[2].ClassList.Should().NotContain("active");

            pageItems[3].FirstElementChild!.Id.Should().Be("LastPage"); // click the last page button
            pageItems[3].FirstElementChild!.Click();

            pageItems = cut.FindAll(".page-item");
            pageItems[2].ClassList.Should().Contain("active");
        }

        [Fact]
        public async Task Should_FireOnPageChangedEvent()
        {
            int currentPage = 1;
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<Pager>(parameters => parameters
                .Add(p => p.TotalItemCount, 11)
                .Add(p => p.OnPageChanged, pg => { currentPage = pg; }));

            var pageItems = cut.FindAll(".page-item");
            await pageItems[2].FirstElementChild!.ClickAsync(new MouseEventArgs { Button = 0 });

            currentPage.Should().Be(2);
        }
    }
}
