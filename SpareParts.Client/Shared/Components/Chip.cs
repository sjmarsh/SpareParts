namespace SpareParts.Client.Shared.Components
{
    public class Chip
    {
        public Chip(string name, bool isActive, string? color = null, string? tooltip = null)
        {
            Name = name;
            IsActive = isActive;
            Color = color;
            Tooltip = tooltip;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? Color { get; set; }
        public string? Tooltip { get; set; }
    }

    public enum ChipColor
    {
        Default,
        AliceBlue,
        HoneyDew,
        Lavender,
        LavenderBlush,
        Linen,
        PowderBlue,
        Thistle
    }
}
