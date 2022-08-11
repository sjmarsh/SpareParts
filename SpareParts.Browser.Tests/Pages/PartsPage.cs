namespace SpareParts.Browser.Tests.Pages
{
    public class PartsPage
    {
        public const string UrlPath = "part-list";
        private readonly IPage _page;
        private string _baseUrl;

        public PartsPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl;
        }

        public async Task InitializePage()
        {
            await _page.GotoAsync($"{_baseUrl}/{UrlPath}");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("h3 >> text=Part List");
        }

        public async Task<string> PageHeader()
        {
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }

        public async Task<int> PartListItemCount(bool waitForList = true)
        {
            if (waitForList)
            {
                await _page.WaitForSelectorAsync("#partList");
            }
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

        public async Task ClickAddButton()
        {
            var addButton = _page.Locator("text=Add");
            addButton.Should().NotBeNull();
            await addButton.ClickAsync();
        }

        public async Task<Shared.Models.Part> GetPartFromRow(int row)
        {
            var cells = _page.Locator("tr").Nth(row).Locator("td");
            
            return new Shared.Models.Part
            {
                Name = await cells.Nth(0).InnerTextAsync(),
                Description = await cells.Nth(1).InnerTextAsync(),
            };
        }

        public async Task<PartModal> GetPartModal()
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

        public async Task EnterName(string? partName)
        {
            if (partName == null) return;
            await EnterValue(nameof(partName), partName);
        }

        public async Task EnterDescription(string? partDescription)
        {
            if (partDescription == null) return;
            await EnterValue(nameof(partDescription), partDescription);
        }

        public async Task EnterWeight(double weight)
        {
            await EnterValue(nameof(weight), weight.ToString());
        }

        public async Task EnterPrice(double price)
        {
            await EnterValue(nameof(price), price.ToString());
        }

        public async Task EnterStartDate(DateTime startDate)
        {
            await EnterValue(nameof(startDate), startDate.ToString("yyyy-MM-dd"));
        }

        public async Task EnterEndDate(DateTime? endDate)
        {
            if (endDate == null) return;
            await EnterValue(nameof(endDate), endDate.Value.ToString("yyyy-MM-dd"));
        }

        private async Task EnterValue(string id, string value)
        {
            await _page.Locator($"#{id}").FillAsync(value);
        }

        public async Task Submit()
        {
            var submitBtn = _page.Locator("text=Submit");
            await submitBtn.ClickAsync();
            await _page.WaitForSelectorAsync(".alert-success");
        }

        public async Task Close()
        {
            var closeBtn = _page.Locator("text=Close");
            await closeBtn.ClickAsync();
            var modal = _page.Locator(".modal-dialog");
            var isModalVisible = await modal.IsVisibleAsync();
            isModalVisible.Should().BeFalse();
        }
    }
}
