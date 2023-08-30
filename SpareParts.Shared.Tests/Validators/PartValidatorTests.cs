namespace SpareParts.Shared.Tests.Validators
{
    public class PartValidatorTests
    {
        private PartValidator _validator;

        public PartValidatorTests()
        {
            _validator = new PartValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorForNoName()
        {
            var result = _validator.TestValidate(new Part());
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForName()
        {
            var result = _validator.TestValidate(new Part { Name = "Bob" });
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ShouldHaveValidationErrorForZeroPrice()
        {
            var result = _validator.TestValidate(new Part { Price = 0 });
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void ShouldHaveValidationErrorForNegativePrice()
        {
            var result = _validator.TestValidate(new Part { Price = -0.1 });
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForPrice()
        {
            var result = _validator.TestValidate(new Part { Price = 1.2 });
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void ShouldHaveValidationErrorForZeroWeight()
        {
            var result = _validator.TestValidate(new Part { Weight = 0 });
            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void ShouldHaveValidationErrorForNegativeWeight()
        {
            var result = _validator.TestValidate(new Part { Weight = -0.1 });
            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForWeight()
        {
            var result = _validator.TestValidate(new Part { Weight = 1.3 });
            result.ShouldNotHaveValidationErrorFor(x => x.Weight);
        }

        [Fact]
        public void ShouldHaveValidationErrorForStartDateBefore2000()
        {
            var result = _validator.TestValidate(new Part { StartDate = new DateTime(1999, 12, 31) });
            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForStartDateAfter2000()
        {
            var result = _validator.TestValidate(new Part { StartDate = new DateTime(2000, 01, 01) });
            result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void ShouldHaveValidationErrorForInvalidAttribute()
        {
            var result = _validator.TestValidate(new Part { Attributes = new List<PartAttribute> { new PartAttribute() } });
            result.ShouldHaveValidationErrorFor(x => x.Attributes[0].Name);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorForValidAttribute()
        {
            var result = _validator.TestValidate(new Part { Name = "Part1", Attributes = new List<PartAttribute> { new PartAttribute { Name = "Colour", Value = "Red" } } });
            result.ShouldNotHaveValidationErrorFor(x => x.Attributes[0].Name);
        }
    }
}