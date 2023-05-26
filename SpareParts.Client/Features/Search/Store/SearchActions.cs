using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Features.Search.Store
{
    public class InitializeFilterFieldsAction : StateChangeAction<List<FilterField>>
    {
        public InitializeFilterFieldsAction(List<FilterField> payload) : base(payload)
        {
        }
    }

    public class AddFilterFieldAction : StateChangeAction<FilterField>
    {
        public AddFilterFieldAction(FilterField payload) : base(payload)
        {
        }
    }

    public class RemoveFilterFieldAction : StateChangeAction<FilterField>
    {
        public RemoveFilterFieldAction(FilterField payload) : base(payload)
        {
        }
    }

    public class ToggleFilterFieldAction : StateChangeAction<string>
    {
        public ToggleFilterFieldAction(string filterFieldName) : base(filterFieldName)
        {
        }
    }

    public class InitializeFilterLinesAction : StateChangeAction<List<FilterLine>>
    {
        public InitializeFilterLinesAction(List<FilterLine> payload) : base(payload)
        {
        }
    }

    public class AddFilterLineAction : StateChangeAction<FilterLine>
    {
        public AddFilterLineAction(FilterLine payload) : base(payload)
        {
        }
    }

    public class RemoveFilterLineAction : StateChangeAction<FilterLine>
    {
        public RemoveFilterLineAction(FilterLine payload) : base(payload)
        {
        }
    }    
}
