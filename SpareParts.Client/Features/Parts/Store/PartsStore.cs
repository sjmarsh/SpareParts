using Fluxor;

namespace SpareParts.Client.Features.Parts.Store
{
    public record PartsState
    {
        public int PartsPage { get; init; }
        public int? SelectedPartId { get; init; }
        public bool IsDetailModeVisible { get; init; }
    }

    public class PartsFeature : Feature<PartsState>
    {
        public override string GetName() => "Parts";

        protected override PartsState GetInitialState()
        {
            return new PartsState 
            { 
                PartsPage = 1,
                SelectedPartId = null,
                IsDetailModeVisible = false
            };
        }
    }

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
