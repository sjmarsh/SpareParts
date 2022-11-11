namespace SpareParts.Client.Features.Inventory.Store
{
    public class SetSelectedInventoryTabAction : StateChangeAction<int>
    {
        public SetSelectedInventoryTabAction(int payload) : base(payload)
        {
        }
    }

    public class SetCurrentInventoryPageAction : StateChangeAction<int>
    {
        public SetCurrentInventoryPageAction(int payload) : base(payload)
        {
        }
    }

    public class SetHistoryPageAction : StateChangeAction<int>
    {
        public SetHistoryPageAction(int payload) : base(payload)
        {
        }
    }
}
