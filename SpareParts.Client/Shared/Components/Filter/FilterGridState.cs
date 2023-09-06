using System.Collections.Immutable;

namespace SpareParts.Client.Shared.Components.Filter
{
    public record FilterGridState
    {
        public FilterGridState()
        {
            FilterFields = ImmutableList<FilterField>.Empty;
            FilterLines = ImmutableList<FilterLine>.Empty;
            CurrentResultPage = 1;
        }

        public ImmutableList<FilterField> FilterFields { get; set; }
        public ImmutableList<FilterLine> FilterLines { get; set; }
        public int CurrentResultPage { get; set; }
    }
}
