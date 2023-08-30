using FluentValidation;
using SpareParts.Shared.Models;

namespace SpareParts.Shared.Validators
{
    public class PartValidator : AbstractValidator<Part>
    {
        public PartValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.Weight).GreaterThan(0);
            RuleFor(p => p.StartDate).GreaterThanOrEqualTo(new DateTime(2000, 01, 01));
            RuleForEach(p => p.Attributes).SetValidator(new PartAttributeValidator());
        }
    }
}
