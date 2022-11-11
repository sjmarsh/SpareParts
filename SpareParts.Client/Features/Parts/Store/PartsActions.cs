namespace SpareParts.Client.Features.Parts.Store
{

    public class SetPartsPageAction : StateChangeAction<int>
    {
        public SetPartsPageAction(int payload) : base(payload)
        {
        }
    }

    public class SetSelectedPartIdAction : StateChangeAction<int?>
    {
        public SetSelectedPartIdAction(int? payload) : base(payload)
        {
        }
    }

    public class SetPartsDetailVisibilityAction : StateChangeAction<bool>
    {
        public SetPartsDetailVisibilityAction(bool payload) : base(payload)
        {
        }
    }

}
