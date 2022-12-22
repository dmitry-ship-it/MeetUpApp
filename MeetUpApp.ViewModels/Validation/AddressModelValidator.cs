using FluentValidation;

namespace MeetUpApp.ViewModels.Validation
{
    public class AddressModelValidator : AbstractValidator<AddressViewModel>
    {
        public AddressModelValidator()
        {
            RuleFor(a => a.Сountry).NotEmpty();

            RuleFor(a => a.City).NotEmpty();
            RuleFor(a => a.Street).NotEmpty();
            RuleFor(a => a.House).NotEmpty().MaximumLength(20);
            RuleFor(a => a.PostCode).Length(6);
        }
    }
}
