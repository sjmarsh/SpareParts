using Fluxor;
using SpareParts.Client.Shared.Components.Filter;
using System.Collections.Immutable;

namespace SpareParts.Client.Features.Search.Store
{
    public record SearchState
    {
        public ImmutableList<FilterField> FilterFields { get; set; } = ImmutableList<FilterField>.Empty;
        public ImmutableList<FilterLine> FilterLines { get; set; } = ImmutableList<FilterLine>.Empty;
    }

    public class SearchFeature : Feature<SearchState>
    {
        public override string GetName() => "Search";
        
        protected override SearchState GetInitialState()
        {
            return new SearchState 
            { 
                FilterFields = ImmutableList<FilterField>.Empty,
                FilterLines = ImmutableList<FilterLine>.Empty,
            };
        }
    }
}
