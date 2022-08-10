namespace SpareParts.Browser.Tests.Pages
{
    public class PartsPage
    {
        public const string UrlPath = "part-list";
        private readonly IPage _page;

        public PartsPage(IPage page)
        {
            _page = page;
        }

        private async Task EnsureOnPartsPage()
        {
            if (_page.Url.Contains(UrlPath))
            {
                return;
            }

            await new NavBar(_page).ClickPartsNav();
        }

        public async Task<string> PageHeader()
        {
            await EnsureOnPartsPage();
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }

        public async Task<bool> PartListHasItems()
        {
            await EnsureOnPartsPage();
            await _page.WaitForSelectorAsync("tbody >> tr");
            var rows = await _page.QuerySelectorAllAsync("tbody >> tr");
            return rows.Count() > 0;
        }

        public async Task<int> PartListItemCount()
        {
            await EnsureOnPartsPage();
            var rows = await _page.QuerySelectorAllAsync("tbody >> tr");
            return rows.Count();
        }

        public async Task ClickEditButtonForRow(int row)
        {
            await EnsureOnPartsPage();
            var editButtons = await _page.QuerySelectorAllAsync(".btn-link >> text=Edit");
            editButtons.Should().NotBeNullOrEmpty();
            await editButtons[row].ClickAsync();
        }

        public async Task ClickAddButton()
        {
            await EnsureOnPartsPage();
            var addButton = _page.Locator("text=Add");
            addButton.Should().NotBeNull();
            await addButton.ClickAsync();
        }

        public async Task<PartModal> GetPartModal()
        {
            await EnsureOnPartsPage();
            var modal = _page.Locator(".modal-dialog");
            modal.Should().NotBeNull();
           // (modal.IsVisibleAsync()).Should().Be(true);
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
            await _page.WaitForSelectorAsync(".alert");
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
