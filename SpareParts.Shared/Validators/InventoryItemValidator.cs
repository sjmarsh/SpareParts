using FluentValidation;
using SpareParts.Shared.Models;

namespace SpareParts.Shared.Validators
{
    public class InventoryItemValidator : AbstractValidator<InventoryItem>
    {
        public InventoryItemValidator()
        {
            RuleFor(i => i.PartID).GreaterThan(0).WithMessage("Part must be selected for Inventory Item");
            RuleFor(i => i.Quantity).GreaterThanOrEqualTo(0);
        }
    }
}
