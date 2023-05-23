using FluentValidation;

namespace SpareParts.Client.Shared.Components.Filter
{
    public class FilterLineValidator : AbstractValidator<FilterLine>
    {
        public FilterLineValidator()
        {
            RuleFor(f => f.SelectedField).NotNull().NotEmpty();
            RuleFor(f => f.SelectedOperator).NotNull().NotEmpty();
            RuleFor(f => f.Value).NotNull().NotEmpty();
        }
    }

    public class FilterLinesValidator : AbstractValidator<List<FilterLine>>
    {
        public FilterLinesValidator()
        {
            RuleForEach(l => l).SetValidator(new FilterLineValidator());
        }
    }
}
