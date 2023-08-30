namespace SpareParts.Shared.Tests.Validators
{
    public class PartAttributeValidatorTests
    {
        private readonly PartAttributeValidator _validator;

        public PartAttributeValidatorTests()
        {
            _validator = new PartAttributeValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorForNoName()
        {
            var result = _validator.TestValidate(new PartAttribute());
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShoulNotdHaveValidationErrorForName()
        {
            var result = _validator.TestValidate(new PartAttribute { Name = "Colour" });
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShouldHaveValidationErrorForNoValue()
        {
            var result = _validator.TestValidate(new PartAttribute());
            result.ShouldHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        public void ShoulNotdHaveValidationErrorForValue()
        {
            var result = _validator.TestValidate(new PartAttribute { Value = "Red" });
            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }
    }
}
