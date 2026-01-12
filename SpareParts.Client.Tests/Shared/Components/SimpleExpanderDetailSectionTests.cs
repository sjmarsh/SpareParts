using SpareParts.Client.Shared.Components.DataGrid;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class SimpleExpanderDetailSectionTests
    {
        [Fact]
        public void Should_RenderDetailForGivenDataSource()
        {
            var detailHeader = "The Details";
            var row1 = new Dictionary<string, string> { { "Name", "The Name" }, { "Value", "The Value" } };
            var row2 = new Dictionary<string, string> { { "Name", "Other Name" }, { "Value", "Other Value" } };
            var data = new List<Dictionary<string, string>> { row1, row2 };
            var dataRowDetail1 = new DataRowDetail(detailHeader, data);
            var detailRows = new List<DataRowDetail> { dataRowDetail1 };

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleExpanderDetailSection>(parameters => parameters
                .Add(p => p.DetailRows,detailRows));

            var table = cut.Find("table");
            table.Should().NotBeNull();

            var header = cut.Find("h6");
            header.Should().NotBeNull();
            header.TextContent.Should().Be(detailHeader);

            var rows = cut.FindAll("tr");
            rows.Should().NotBeNull();
            rows.Should().HaveCount(3);

            var headers = cut.FindAll("th");
            headers.Should().NotBeNull();
            headers.Should().HaveCount(2);
            headers.Select(h => h.TextContent).Should().BeEquivalentTo(new[] { "Name", "Value" });

            var dataCells = cut.FindAll("td");
            dataCells.Should().NotBeNull();
            dataCells.Should().HaveCount(4);
            dataCells.Select(d => d.TextContent).Should().BeEquivalentTo(new[] { "The Name", "The Value", "Other Name", "Other Value" });
        }
    }
}
