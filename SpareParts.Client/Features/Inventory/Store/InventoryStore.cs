using Fluxor;

namespace SpareParts.Client.Features.Inventory.Store
{
    public record InventoryState
    {
        public int SelectedInventoryTab { get; init; }
        public int CurrentInventoryPage { get; init; }
        public int HistoryPage { get; init; }
    }

    public class InventoryFeature : Feature<InventoryState>
    {
        public override string GetName() => "Inventory";

        protected override InventoryState GetInitialState()
        {
            return new InventoryState 
            { 
                SelectedInventoryTab = 0,
                CurrentInventoryPage = 1,
                HistoryPage = 1
            };
        }
    }


}
