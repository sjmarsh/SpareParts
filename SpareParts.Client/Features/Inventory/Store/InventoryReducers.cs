using Fluxor;

namespace SpareParts.Client.Features.Inventory.Store
{
    public static class InventoryReducers
    {
        [ReducerMethod]
        public static InventoryState OnSetIventoryTab(InventoryState state, SetSelectedInventoryTabAction action)
        {
            return state with
            {
                SelectedInventoryTab = action.Payload
            };
        }

        [ReducerMethod]
        public static InventoryState OnSetCurrentInventoryPage(InventoryState state, SetCurrentInventoryPageAction action)
        {
            return state with
            {
                CurrentInventoryPage = action.Payload
            };
        }

        [ReducerMethod]
        public static InventoryState OnSetHistoryPage(InventoryState state, SetHistoryPageAction action)
        {
            return state with
            {
                HistoryPage = action.Payload
            };
        }
    }
}
