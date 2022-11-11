using Fluxor;

namespace SpareParts.Client.Features.Parts.Store
{
    public static class PartsReducers
    {
        [ReducerMethod]
        public static PartsState OnSetPartsPage(PartsState state, SetPartsPageAction action)
        {
            return state with
            {
                PartsPage = action.Payload
            };
        }

        [ReducerMethod]
        public static PartsState OnSetSelectedPartId(PartsState state, SetSelectedPartIdAction action)
        {
            return state with
            {
                SelectedPartId = action.Payload
            };
        }

        [ReducerMethod]
        public static PartsState OnSetDetailVisibility(PartsState state, SetPartsDetailVisibilityAction action)
        {
            return state with
            {
                IsDetailModeVisible = action.Payload
            };
        }
    }   
}
