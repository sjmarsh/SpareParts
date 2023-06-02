using Fluxor;

namespace SpareParts.Client.Features.Search.Store
{
    public class SearchReducers
    {
        [ReducerMethod]
        public static SearchState OnUpdateSearchState(SearchState state, UpdateSearchStateAction action) 
        {
            return state with
            {
                FilterGridState = action.Payload
            };
        }
    }
}
