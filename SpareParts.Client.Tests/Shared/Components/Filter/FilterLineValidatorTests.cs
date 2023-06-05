using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Tests.Shared.Components.Filter
{
    public class FilterLineValidatorTests
    {
        private readonly FilterLineValidator _validator;

        public FilterLineValidatorTests()
        {
            _validator = new FilterLineValidator();
        }

        [Fact]
        public void Should_HaveErrorsForInvalidFilterLine()
        {
            var result = _validator.Validate(new FilterLine());
            
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Contain("'Value' must not be empty.");
        }

        [Fact]
        public void Should_HaveNoErrorsForValidFilterLine()
        {
            var result = _validator.Validate(new FilterLine(new FilterField("Test", typeof(string), true), FilterOperator.Equal, "Test"));

            result.Errors.Should().BeEmpty();
        }
    }
}
