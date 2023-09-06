namespace SpareParts.Client.Shared.Components.Buttons
{
    public enum ButtonType
    {
        Button,
        Submit,
        Reset
    }

    public static class ButtonTypeExtensions
    {
        public static string MarkupValue(this ButtonType iconButtonType)
        {
            return iconButtonType.ToString().ToLower();
        }
    }
}
