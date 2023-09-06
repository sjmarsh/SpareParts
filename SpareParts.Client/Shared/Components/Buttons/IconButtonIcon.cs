using System.ComponentModel;
using SpareParts.Shared.Extensions;

namespace SpareParts.Client.Shared.Components.Buttons
{
    public enum IconButtonIcon
    {
        [Description("oi oi-chevron-bottom")]
        ChevronBottom,
        [Description("oi oi-chevron-top")]
        ChevronTop,
        [Description("oi oi-magnifying-glass")]
        MagnifyingGlass,
        [Description("oi oi-plus")]
        Plus,
        [Description("oi oi-print")]
        Print
    }

    public static class IconButtonIconExtensions
    {
        public static string GetCss(this IconButtonIcon iconButton)
        {
            var description = iconButton.GetEnumDescription();
            return description ?? "";
        }
    }   

}
