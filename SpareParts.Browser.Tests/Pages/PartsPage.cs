using SpareParts.Shared.Models;
using System.Text.RegularExpressions;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SpareParts.Browser.Tests.Pages
{
    public class PartsPage
    {
        public const string UrlPath = "part-list";
        private readonly IPage _page;
        private readonly string _baseUrl;
        private readonly NavBar _navBar;

        public PartsPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl;
            _navBar = new NavBar(_page);
        }

        public async Task InitializePage()
        {
            await _navBar.ClickHomeNav();
            await _navBar.ClickPartsNav();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("h3 >> text=Part List");
        }

        public async Task<string> PageHeader()
        {
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }

        public async Task<bool> IsPartTableVisible()
        {
            var partTableCount = await _page.Locator("#partTable").CountAsync();
            return partTableCount == 1;
        }
        
        public async Task<int> PartListItemCount()
        {
            await _page.WaitForSelectorAsync("#partTable");
            var rows = await _page.QuerySelectorAllAsync("tbody >> tr");
            return rows.Count();
        }

        public async Task ClickEditButtonForRow(int row)
        {
            var editButtons = await _page.QuerySelectorAllAsync("text=Edit");
            editButtons.Should().NotBeNullOrEmpty();
            await editButtons[row].ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task ClickDeleteButtonForRow(int row)
        {
            var deleteButtons = await _page.QuerySelectorAllAsync("text=Delete");
            deleteButtons.Should().NotBeNullOrEmpty();
            await deleteButtons[row].FocusAsync();
            await deleteButtons[row].ClickAsync(new ElementHandleClickOptions { ClickCount = 1, Delay = 200 });
            await _page.WaitForResponseAsync(r => r.Status == 200);    
        }

        public async Task ClickAddButton()
        {
            var addButton = _page.Locator("text=Add Part");
            addButton.Should().NotBeNull();
            await addButton.ClickAsync();
        }

        public async Task<Shared.Models.Part> GetPartFromRow(int row)
        {
            var tableRow = _page.Locator("tr").Nth(row + 1);  // zero is header
            var cells = tableRow.Locator("td");
                        
            return new Shared.Models.Part
            {
                Name = await cells.Nth(0).InnerTextAsync(),
                Description = await cells.Nth(1).InnerTextAsync(),
                Weight = GetDouble(await cells.Nth(2).InnerTextAsync()),
                Price = GetDouble((await cells.Nth(3).InnerTextAsync()).Replace("$", "")),
                StartDate = GetDate(await cells.Nth(4).InnerTextAsync())!.Value.Date,
                EndDate = GetDate(await cells.Nth(5).InnerTextAsync())
            };
        }

        private double GetDouble(string doubleString)
        {
            var cleanString = Regex.Replace(doubleString, "[^0-9.]", "");
            return double.Parse(cleanString);
        }

        private DateTime? GetDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString)) return null;

            return Convert.ToDateTime(dateString);
        }


        public PartModal GetPartModal()
        {
            var modal = _page.Locator(".modal-dialog");
            modal.Should().NotBeNull();
            
            return new PartModal(_page);
        }
    }

    public class PartModal
    {
        private readonly IPage _page;

        public PartModal(IPage page)
        {
            _page = page;
        }

        public async Task<Part> GetPart()
        {
            return new Part
            {
                Name = await GetName(),
                Description = await GetDescription(),
                Weight = await GetWeight(),
                Price = await GetPrice(),
                StartDate = await GetStartDate(),
                EndDate = await GetEndDate(),
                Attributes = await GetAttributes()
            };
        }

        public async Task<string> GetName()
        {
            return await GetValue("partName");
        }

        public async Task EnterName(string? partName)
        {
            if (partName == null) return;
            await EnterValue(nameof(partName), partName);
        }

        public async Task<string> GetDescription()
        {
            return await GetValue("partDescription");
        }

        public async Task EnterDescription(string? partDescription)
        {
            if (partDescription == null) return;
            await EnterValue(nameof(partDescription), partDescription);
        }

        public async Task<double> GetWeight()
        {
            var weight = await GetValue(nameof(Part.Weight).ToLower());
            var parsed = double.TryParse(weight, out var result);
            return parsed ? result : 0.0;
        }

        public async Task EnterWeight(double weight)
        {
            await EnterValue(nameof(weight), weight.ToString());
        }

        public async Task<double> GetPrice()
        {
            var price = await GetValue(nameof(Part.Price).ToLower());
            if (string.IsNullOrEmpty(price)) return 0.0;
            price = price.Replace("$", "");
            var parsed = double.TryParse(price, out var result);
            return parsed ? result : 0.0;
        }

        public async Task EnterPrice(double price)
        {
            await EnterValue(nameof(price), price.ToString());
        }

        private async Task<DateTime> GetStartDate()
        {
            var startDate = await GetValue(nameof(Part.StartDate).Camelize());
            var parsed = DateTime.TryParse(startDate, out var result);
            return parsed ? result : DateTime.MinValue;
        }

        public async Task EnterStartDate(DateTime startDate)
        {
            await EnterValue(nameof(startDate), startDate.ToString("yyyy-MM-dd"));
        }

        private async Task<DateTime?> GetEndDate()
        {
            var endDate = await GetValue(nameof(Part.EndDate).Camelize());
            if(string.IsNullOrEmpty(endDate)) return null;
            var parsed = DateTime.TryParse(endDate, out var result);
            return parsed ? result : DateTime.MinValue;
        }

        public async Task EnterEndDate(DateTime? endDate)
        {
            if (endDate == null) return;
            await EnterValue(nameof(endDate), endDate.Value.ToString("yyyy-MM-dd"));
        }

        private async Task<string> GetValue(string id)
        {            
            return await _page.Locator($"#{id}").InputValueAsync();
        }

        private async Task EnterValue(string id, string value)
        {
            await _page.Locator($"#{id}").FillAsync(value);
        }

        public async Task ClickShowAttributes()
        {
            var attributesButton = _page.Locator("text=Attributes");
            attributesButton.Should().NotBeNull();
            await attributesButton.ClickAsync();
        }

        public async Task ClickAddAttribute()
        {
            var addAttributeButton = _page.Locator("text=Add Attribute");
            addAttributeButton.Should().NotBeNull();
            await addAttributeButton.ClickAsync();
        }

        public async Task<List<PartAttribute>> GetAttributes()
        {
            var partAttributes = new List<PartAttribute>();
            var attributeSection = _page.Locator("details").Nth(0);
            attributeSection.Should().NotBeNull();
                        
            var attributeRows = await attributeSection.Locator("tr").AllAsync();
            if(attributeRows.Count > 1)
            {
                var numberOfAttributeColumns = typeof(PartAttribute).GetProperties().Count(p => p.Name != "ID");
                var rowCount = 0;
                foreach (var row in attributeRows)
                {
                    rowCount++;
                    if (rowCount == 1) continue; // don't include header row
                    var partAttribute = new PartAttribute();
                    var cells = row.Locator("td");
                    for (int i = 0; i < numberOfAttributeColumns; i++)
                    {
                        var input = cells.Nth(i).Locator("input");
                        switch (i)
                        {
                            case 0:
                                partAttribute.Name = await input.InputValueAsync(); 
                                break;
                            case 1:
                                partAttribute.Description = await input.InputValueAsync();
                                break;
                            case 2:
                                partAttribute.Value = await input.InputValueAsync();
                                break;
                            default:
                                break;
                        }
                    }
                    partAttributes.Add(partAttribute);
                }
            }
            return partAttributes;
        }

        public async Task EnterAttributes(List<PartAttribute> attributes)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                var attribute = attributes[i];
                if (attribute != null)
                {
                    await EnterAttribute(i, attribute);
                }
            }
        }

        public async Task EnterAttribute(int row, PartAttribute attribute)
        {
            attribute.Should().NotBeNull();
            if(attribute != null)
            {
                await EnterAttributeName(row, attribute.Name!);
                await EnterAttributeDescription(row, attribute.Description!);
                await EnterAttributeValue(row, attribute.Value!);
            }
        }

        public async Task EnterAttributeName(int row, string name)
        {
            await EnterAttributeComponent(row, 0, name);
        }

        public async Task EnterAttributeDescription(int row, string description)
        {
            await EnterAttributeComponent(row, 1, description);
        }

        public async Task EnterAttributeValue(int row, string value)
        {
            await EnterAttributeComponent(row, 2, value);
        }

        private async Task EnterAttributeComponent(int row, int column, string componentValue)
        {
            var attributeSection = _page.Locator("details").Nth(0);
            attributeSection.Should().NotBeNull();

            var attributeRow = attributeSection.Locator("tr").Nth(row + 1);
            var cells = attributeRow.Locator("td");
            var input = cells.Nth(column).Locator("input");
            input.Should().NotBeNull();
            await input.FillAsync(componentValue);
        }

        public async Task DeleteAttribute(int row)
        {
            var deleteButtons = await _page.QuerySelectorAllAsync("text=Delete");
            deleteButtons.Should().NotBeNullOrEmpty();
            await deleteButtons[row].FocusAsync();
            await deleteButtons[row].ClickAsync(new ElementHandleClickOptions { ClickCount = 1, Delay = 200 });
            await _page.WaitForResponseAsync(r => r.Status == 200);
        }

        public async Task Submit()
        {
            var submitBtn = _page.Locator("text=Submit");
            await submitBtn.ClickAsync();
            await _page.WaitForSelectorAsync(".alert-success");
        }

        public async Task Close()
        {
            var closeBtn = _page.Locator("#closeModal");
            await closeBtn.ClickAsync();
            
            var modal = _page.Locator(".modal-dialog");
            var isModalVisible = await modal.IsVisibleAsync();
            isModalVisible.Should().BeFalse();            
        }
    }
}
