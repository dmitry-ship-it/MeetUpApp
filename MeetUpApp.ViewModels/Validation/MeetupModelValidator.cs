using FluentValidation;

namespace MeetUpApp.ViewModels.Validation
{
    public class MeetupModelValidator : AbstractValidator<MeetupViewModel>
    {
        public MeetupModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Speaker).NotEmpty();
            RuleFor(m => m.DateTime).NotEmpty();
            RuleFor(m => m.Address).SetValidator(new AddressModelValidator());
        }
    }
}
