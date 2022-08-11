namespace SpareParts.Browser.Tests.Pages
{
    public class PageModels
    {
        private IPage _page;
        private string _baseUrl;
                
        public PageModels(IPage page, string baseUrl) 
        {
            _page = page;
            _baseUrl = baseUrl;
        }

        public NavBar NavBar => new(_page);

        public PartsPage Parts => new(_page, _baseUrl);
    }
}
