namespace SpareParts.Browser.Tests.Pages
{
    public class PageModels
    {
        private readonly IPage _page;
        private readonly string _baseUrl;
                
        public PageModels(IPage page, string baseUrl) 
        {
            _page = page;
            _baseUrl = baseUrl;
        }

        public NavBar NavBar => new(_page);

        public HomePage Home => new(_page);

        public LoginPage Login => new(_page, _baseUrl);

        public PartsPage Parts => new(_page, _baseUrl);

        public InventoryPage Inventory => new(_page, _baseUrl);
    }
}
