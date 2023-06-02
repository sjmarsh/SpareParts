using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Features.Search.Store
{
    public class UpdateSearchStateAction : StateChangeAction<FilterGridState>
    {
        public UpdateSearchStateAction(FilterGridState payload) : base(payload)
        {
        }
    }
}
