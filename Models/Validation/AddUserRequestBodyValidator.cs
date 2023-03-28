using BarcodeAPI.Entities;
using FluentValidation;

namespace BarcodeAPI.Models.Validation
{
    public class AddUserRequestBodyValidator : AbstractValidator<AddUserBodyRequest>
    {
        public AddUserRequestBodyValidator(ProductsDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.Email).Custom(
                (value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Already in use");
                    }
                });
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }
}