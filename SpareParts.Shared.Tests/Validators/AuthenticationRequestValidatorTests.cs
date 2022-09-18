namespace SpareParts.Shared.Tests.Validators
{
    public class AuthenticationRequestValidatorTests
    {
        private AuthenticationRequestValidator _validator;

        public AuthenticationRequestValidatorTests()
        {
            _validator = new AuthenticationRequestValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorWhenNoUserName()
        {
            var result = _validator.TestValidate(new AuthenticationRequest("", "password"));
            result.ShouldHaveValidationErrorFor(a => a.UserName);
        }

        [Fact]
        public void ShouldHaveValidationErrorWhenNoPassword()
        {
            var result = _validator.TestValidate(new AuthenticationRequest("user", ""));
            result.ShouldHaveValidationErrorFor(a => a.Password);
        }

        [Fact]
        public void ShouldNotHaveValidationErrorWhenAllCredentialsProvided()
        {
            var result = _validator.TestValidate(new AuthenticationRequest("user", "password"));
            result.ShouldNotHaveValidationErrorFor(a => a.UserName);
            result.ShouldNotHaveValidationErrorFor(a => a.Password);
        }
    }
}
