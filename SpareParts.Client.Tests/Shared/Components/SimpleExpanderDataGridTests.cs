using SpareParts.Client.Shared.Components.DataGrid;
using SpareParts.Shared.Constants;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class SimpleExpanderDataGridTests
    {
        private const string ShowDetailsButton = ".oi-chevron-top";
        private const string HideDetailButton = ".oi-chevron-bottom";

        [Fact]
        public void Should_RenderGridForGivenDataSource()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);
            List<TestModelWithList> testModels = new()
            {
                new TestModelWithList { DateVal = theDate1, NumberVal = 2, TextVal = "Hello",
                    SomeList = new List<TestDifferentModel>{
                        new TestDifferentModel{ ID = 1, Name = "TheName", Value = "The Value" },
                        new TestDifferentModel{ ID = 2, Name = "OtherName", Value = "Other Value" },
                }},
                new TestModelWithList { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };

            var columnList = new [] { "DateVal", "NumberVal", "TextVal", "Name", "Value" }.Select(c => new ColumnHeader(c)).ToList();
            
            var ctx = new TestContext();
            var cut = ctx.Render<SimpleExpanderDataGrid<TestModelWithList>>(parameters => parameters
                .Add(p => p.DataSource, testModels)
                .Add(p => p.ColumnList, columnList));

            var table = cut.Find("table");
            table.Should().NotBeNull();
            
            var rows = cut.FindAll("tr");
            rows.Should().NotBeNull();
            rows.Should().HaveCount(3);  

            var headers = cut.FindAll("th");
            headers.Should().NotBeNull();
            headers.Should().HaveCount(4); // 3 headings + 1 blank for the expander control column
            headers.Select(h => h.TextContent).Should().BeEquivalentTo(new [] { "", "Date Val", "Number Val", "Text Val" });
            
            var dataCells = cut.FindAll("td");
            dataCells.Should().NotBeNull();
            dataCells.Should().HaveCount(8);
            dataCells.Select(d => d.TextContent).Should().BeEquivalentTo(new[] { "", theDate1.ToString(DefaultStringFormat.ForDate), "2.00", "Hello", "", theDate2.ToString(DefaultStringFormat.ForDate), "4.00", "Goodbye" });

            var showDetailsButtons = cut.FindAll(ShowDetailsButton);
            showDetailsButtons.Should().NotBeNull();
            showDetailsButtons.Should().HaveCount(2);
        }

        [Fact]
        public void Should_RenderDetailSectionWhenShowDetailButtonClicked()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);
            List<TestModelWithList> testModels = new()
            {
                new TestModelWithList { DateVal = theDate1, NumberVal = 2, TextVal = "Hello",
                    SomeList = new List<TestDifferentModel>{
                        new TestDifferentModel{ ID = 1, Name = "TheName", Value = "The Value" },
                        new TestDifferentModel{ ID = 2, Name = "OtherName", Value = "Other Value" },
                }},
                new TestModelWithList { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };

            var columnList = new[] { "DateVal", "NumberVal", "TextVal", "Name", "Value" }.Select(c => new ColumnHeader(c)).ToList();

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleExpanderDataGrid<TestModelWithList>>(parameters => parameters
                .Add(p => p.DataSource, testModels)
                .Add(p => p.ColumnList, columnList));

            var showDetailsButtons = cut.FindAll(ShowDetailsButton);
            showDetailsButtons.Should().NotBeNull();
            showDetailsButtons.Should().HaveCount(2);

            showDetailsButtons[0].Click();

            var tables = cut.FindAll(".table");
            tables.Should().NotBeNull();
            tables.Should().HaveCount(2); // primary outer table + displayed detail table
        }

        [Fact]
        public void Should_HideDetailSectionWhenHideDetailButtonClicked()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);
            List<TestModelWithList> testModels = new()
            {
                new TestModelWithList { DateVal = theDate1, NumberVal = 2, TextVal = "Hello",
                    SomeList = new List<TestDifferentModel>{
                        new TestDifferentModel{ ID = 1, Name = "TheName", Value = "The Value" },
                        new TestDifferentModel{ ID = 2, Name = "OtherName", Value = "Other Value" },
                }},
                new TestModelWithList { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };

            var columnList = new[] { "DateVal", "NumberVal", "TextVal", "Name", "Value" }.Select(c => new ColumnHeader(c)).ToList();

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleExpanderDataGrid<TestModelWithList>>(parameters => parameters
                .Add(p => p.DataSource, testModels)
                .Add(p => p.ColumnList, columnList));

            // show details for the first row
            var showDetailsButtons = cut.FindAll(ShowDetailsButton);
            showDetailsButtons[0].Click();

            var hideDetailButtons = cut.FindAll(HideDetailButton);
            hideDetailButtons.Should().NotBeNull();
            hideDetailButtons.Should().HaveCount(1);  // the button we clicked should now be a hide button

            hideDetailButtons[0].Click();

            var tables = cut.FindAll(".table");
            tables.Should().NotBeNull();
            tables.Should().HaveCount(1); // only the primary outer table should be rendered
        }

        [Fact]
        public void Should_RenderGridForGivenDataSourceWithCustomColumns()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);

            List<TestModelWithList> testModels = new()
            {
                new TestModelWithList { DateVal = theDate1, NumberVal = 2, TextVal = "Hello",
                    SomeList = new List<TestDifferentModel>{
                        new TestDifferentModel{ ID = 1, Name = "TheName", Value = "The Value" },
                        new TestDifferentModel{ ID = 2, Name = "OtherName", Value = "Other Value" },
                }},
                new TestModelWithList { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };
                        
            var columnList = new[] { "DateVal", "TextVal", "Name", }.Select(c => new ColumnHeader(c)).ToList();  // exclude the NumberVal and Value columns

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleExpanderDataGrid<TestModelWithList>>(parameters => parameters
                .Add(p => p.DataSource, testModels)
                .Add(p => p.ColumnList, columnList));

            var table = cut.Find("table");
            table.Should().NotBeNull();

            var rows = cut.FindAll("tr");
            rows.Should().NotBeNull();
            rows.Should().HaveCount(3);

            var headers = cut.FindAll("th");
            headers.Should().NotBeNull();
            headers.Should().HaveCount(3);
            headers.Select(h => h.TextContent).Should().BeEquivalentTo(new[] { "", "Date Val", "Text Val" });

            var dataCells = cut.FindAll("td");
            dataCells.Should().NotBeNull();
            dataCells.Should().HaveCount(6);
            dataCells.Select(d => d.TextContent).Should().BeEquivalentTo(new[] { "", theDate1.ToString(DefaultStringFormat.ForDate), "Hello", "", theDate2.ToString(DefaultStringFormat.ForDate), "Goodbye" });
        }

        [Fact]
        public void Should_RaiseRowClickEventCallback()
        {
            var theDate1 = new DateTime(2023, 06, 03);
            var theDate2 = new DateTime(2022, 01, 06);
            List<TestModelWithList> testModels = new()
            {
                new TestModelWithList { DateVal = theDate1, NumberVal = 2, TextVal = "Hello",
                    SomeList = new List<TestDifferentModel>{
                        new TestDifferentModel{ ID = 1, Name = "TheName", Value = "The Value" },
                        new TestDifferentModel{ ID = 2, Name = "OtherName", Value = "Other Value" },
                }},
                new TestModelWithList { DateVal = theDate2, NumberVal = 4, TextVal = "Goodbye" }
            };

            var columnList = new[] { "DateVal", "NumberVal", "TextVal", "Name", "Value" }.Select(c => new ColumnHeader(c)).ToList();

            DataRow<TestModelWithList>? rowData = null;

            var ctx = new TestContext();
            var cut = ctx.Render<SimpleExpanderDataGrid<TestModelWithList>>(parameters => parameters
                .Add(p => p.DataSource, testModels)
                .Add(p => p.ColumnList, columnList)
                .Add(p => p.OnRowClick, (rowClickData) => rowData = rowClickData));

            var table = cut.Find("table");
            table.Should().NotBeNull();

            var rows = cut.FindAll("tr");
            rows[2].Click(); // last row (inc header row)

            rowData.Should().NotBeNull();
            rowData!.Data["DateVal"].Should().Be(theDate2.ToString(DefaultStringFormat.ForDate));
            rowData!.Data["NumberVal"].Should().Be("4.00");
            rowData!.Data["TextVal"].Should().Be("Goodbye");
        }
    }
}
