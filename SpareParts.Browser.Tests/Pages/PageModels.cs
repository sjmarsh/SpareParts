namespace SpareParts.Browser.Tests.Pages
{
    public class PageModels
    {
        private IPage _page;

        public PageModels(IPage page)
        {
            _page = page;
        }
                
        public NavBar NavBar => new(_page);

        public PartsPage Parts => new(_page);
    }
}
