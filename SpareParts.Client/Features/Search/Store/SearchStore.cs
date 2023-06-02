﻿using Fluxor;
using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Features.Search.Store
{
    public record SearchState
    {
        public FilterGridState FilterGridState { get; set; }
    }

    public class SearchFeature : Feature<SearchState>
    {
        public override string GetName() => "Search";
        
        protected override SearchState GetInitialState()
        {
            return new SearchState
            {
                FilterGridState = new FilterGridState()
            };
        }
    }
}
