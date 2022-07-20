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

    public class InventoryItemDetailValidator : AbstractValidator<InventoryItemDetail>
    {
        public InventoryItemDetailValidator()
        {
            Include(new InventoryItemValidator());
        }
    }

    public class InventoryItemListValidator : AbstractValidator<List<InventoryItem>>
    {
        public InventoryItemListValidator()
        {
            RuleForEach(i => i).SetValidator(new InventoryItemValidator());
        }
    }

    //public class InventoryItemDetailListValidator : AbstractValidator<List<InventoryItemDetail>>
    //{
    //    public InventoryItemDetailListValidator()
    //    {
    //        RuleForEach(i => i).SetValidator(new InventoryItemDetailValidator());
    //    }
    //}

    // Using this to work-around issue validating collections with Fluent & Blazor
    public class StocktakeValidator : AbstractValidator<StocktakeModel>
    {
        public StocktakeValidator()
        {
            RuleForEach(s => s.InventoryItems).SetValidator(new InventoryItemDetailValidator());
        }
    }
}
