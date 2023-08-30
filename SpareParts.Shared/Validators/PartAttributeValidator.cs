using FluentValidation;
using SpareParts.Shared.Models;

namespace SpareParts.Shared.Validators
{
    public class PartAttributeValidator : AbstractValidator<PartAttribute>
    {
        public PartAttributeValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Value).NotEmpty();
        }
    }
}
