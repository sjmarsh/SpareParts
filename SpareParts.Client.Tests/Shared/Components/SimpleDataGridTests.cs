using SpareParts.Client.Shared.Components.DataGrid;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class SimpleDataGridTests
    {
        [Fact]
        public void Should_RenderGridForGivenDataSource()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);

            List<TestModel> testModelList = new()
            {
                new TestModel { DateVal = theDate1, NumberVal = 2, TextVal = "Hello", EnumVal = TestClientEnum.One },
                new TestModel { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye", EnumVal = TestClientEnum.Two }
            };

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleDataGrid<TestModel>>(parameters => parameters
                .Add(p => p.DataSource, testModelList));

            var table = cut.Find("table");
            table.Should().NotBeNull();
            
            var rows = cut.FindAll("tr");
            rows.Should().NotBeNull();
            rows.Should().HaveCount(3);

            var headers = cut.FindAll("th");
            headers.Should().NotBeNull();
            headers.Should().HaveCount(4);
            headers.Select(h => h.TextContent).Should().BeEquivalentTo(new[] { "DateVal", "NumberVal", "TextVal", "EnumVal" });
            
            var dataCells = cut.FindAll("td");
            dataCells.Should().NotBeNull();
            dataCells.Should().HaveCount(8);
            dataCells.Select(d => d.TextContent).Should().BeEquivalentTo(new[] { theDate1.ToString(), "2", "Hello", "One", theDate2.ToString(), "4", "Goodbye", "Two" });
        }

        [Fact]
        public void Should_RenderGridForGivenDataSourceWithCustomColumns()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);

            List<TestModel> testModelList = new()
            {
                new TestModel { DateVal = theDate1, NumberVal = 2, TextVal = "Hello" },
                new TestModel { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };

            List<string> theColumnList = new() { "DateVal", "TextVal" }; // exclude the NumberVal column

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleDataGrid<TestModel>>(parameters => parameters
                .Add(p => p.DataSource, testModelList)
                .Add(p => p.ColumnList, theColumnList));

            var table = cut.Find("table");
            table.Should().NotBeNull();

            var rows = cut.FindAll("tr");
            rows.Should().NotBeNull();
            rows.Should().HaveCount(3);

            var headers = cut.FindAll("th");
            headers.Should().NotBeNull();
            headers.Should().HaveCount(2);
            headers.Select(h => h.TextContent).Should().BeEquivalentTo(new[] { "DateVal", "TextVal" });

            var dataCells = cut.FindAll("td");
            dataCells.Should().NotBeNull();
            dataCells.Should().HaveCount(4);
            dataCells.Select(d => d.TextContent).Should().BeEquivalentTo(new[] { theDate1.ToString(), "Hello", theDate2.ToString(), "Goodbye" });
        }

        [Fact]
        public void Should_RaiseRowClickEventCallback()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);

            Dictionary<string, string>? rowData = null;

            List<TestModel> testModelList = new()
            {
                new TestModel { DateVal = theDate1, NumberVal = 2, TextVal = "Hello" },
                new TestModel { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleDataGrid<TestModel>>(parameters => parameters
                .Add(p => p.DataSource, testModelList)
                .Add(p => p.OnRowClick, (rowClickData) => rowData = rowClickData));

            var table = cut.Find("table");
            table.Should().NotBeNull();

            var rows = cut.FindAll("tr");
            rows[2].Click(); // last row (inc header row)

            rowData.Should().NotBeNull();
            rowData!["DateVal"].Should().Be(theDate2.ToString());
            rowData!["NumberVal"].Should().Be("4");
            rowData!["TextVal"].Should().Be("Goodbye");
        }
    }
}
