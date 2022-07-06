namespace SpareParts.Shared.Tests.Validators
{
    public class InventoryItemValidatorTests
    {
        private InventoryItemValidator _validator;

        public InventoryItemValidatorTests()
        {
            _validator = new InventoryItemValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorWhenNoPartSelected()
        {
            var result = _validator.TestValidate(new InventoryItem { PartID = 0 });
            result.ShouldHaveValidationErrorFor(i => i.PartID);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorWhenPartIsSelected()
        {
            var result = _validator.TestValidate(new InventoryItem { PartID = 1 });
            result.ShouldNotHaveValidationErrorFor(i => i.PartID);
        }

        [Fact]
        public void ShouldHaveValidationErrorForQuantityLessThanZero()
        {
            var result = _validator.TestValidate(new InventoryItem { Quantity = -1 });
            result.ShouldHaveValidationErrorFor(i => i.Quantity);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForQuantityIsZero()
        {
            var result = _validator.TestValidate(new InventoryItem { Quantity = 0 });
            result.ShouldNotHaveValidationErrorFor(i => i.Quantity);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForQuantityGreaterThanZero()
        {
            var result = _validator.TestValidate(new InventoryItem { Quantity = 1 });
            result.ShouldNotHaveValidationErrorFor(i => i.Quantity);
        }

    }
}
