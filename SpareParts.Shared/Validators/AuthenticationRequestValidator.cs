using FluentValidation;
using SpareParts.Shared.Models;

namespace SpareParts.Shared.Validators
{
    public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationRequestValidator()
        {
            RuleFor(a => a.UserName).NotEmpty();
            RuleFor(a => a.Password).NotEmpty();
        }
    }
}
