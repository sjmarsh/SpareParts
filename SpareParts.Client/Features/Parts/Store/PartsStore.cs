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
}
