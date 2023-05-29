using System.Collections.Immutable;
using Fluxor;
using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Features.Search.Store
{
    public class SearchReducers
    {
        [ReducerMethod]
        public static SearchState OnInitiailizeFilterFields(SearchState state, InitializeFilterFieldsAction action) 
        {
            if (state.FilterFields.Any())
            {
                return state;
            }

            return state with
            {
                FilterFields = ImmutableList.Create(action.Payload.ToArray())
            };
        }

        [ReducerMethod]
        public static SearchState OnAddFilterField(SearchState state, AddFilterFieldAction action)
        {
            return state with
            {
                FilterFields = state.FilterFields.Add(action.Payload)
            };
        }

        [ReducerMethod]
        public static SearchState OnRemoveFilterField(SearchState state, RemoveFilterFieldAction action)
        {
            var filterFieldToRemove = state.FilterFields.FirstOrDefault(f => f.Name == action.Payload.Name);

            if (filterFieldToRemove != null)
            {
                return state with
                {
                    FilterFields = state.FilterFields.Remove(filterFieldToRemove)
                };
            }

            return state;
        }

        [ReducerMethod]
        public static SearchState OnToggleFilterField(SearchState state, ToggleFilterFieldAction action)
        {
            var itemToToggle = state.FilterFields.FirstOrDefault(f => f.Name == action.Payload);

            if(itemToToggle != null)
            {
                return state with
                {
                    FilterFields = state.FilterFields.Replace(itemToToggle, new FilterField(itemToToggle.Name, itemToToggle.Type, !itemToToggle.IsSelected))
                };
            }
            return state;
        }

        [ReducerMethod]
        public static SearchState OnInitiailizeFilterLines(SearchState state, InitializeFilterLinesAction action)
        {
            if (state.FilterLines.Any())
            {
                return state;
            }

            return state with
            {
                FilterLines = ImmutableList.Create(action.Payload.ToArray())
            };
        }

        [ReducerMethod]
        public static SearchState OnAddFilterLine(SearchState state, AddFilterLineAction action)
        {
            return state with
            {
                FilterLines = state.FilterLines.Add(action.Payload)
            };
        }

        [ReducerMethod]
        public static SearchState OnUpdateFilterLine(SearchState state, UpdateFilterLineAction action)
        {
            var filterToUpdate = state.FilterLines.FirstOrDefault(f => f.ID == action.Payload.ID);
            if (filterToUpdate != null)
            {
                return state with
                {
                    FilterLines = state.FilterLines.Replace(filterToUpdate, action.Payload)
                };
            }
                        
            return state;
        }

        [ReducerMethod]
        public static SearchState OnRemoveFilterLine(SearchState state, RemoveFilterLineAction action)
        {
            var filterLineToRemove = state.FilterLines.FirstOrDefault(f => f.ID == action.Payload.ID);

            if(filterLineToRemove != null)
            {
                return state with
                {
                    FilterLines = state.FilterLines.Remove(filterLineToRemove)
                };
            }

            return state;
        }
    }
}
