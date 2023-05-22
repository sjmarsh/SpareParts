namespace SpareParts.Client.Shared.Components
{
    public class Chip
    {
        public Chip(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
