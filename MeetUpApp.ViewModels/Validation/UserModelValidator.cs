using FluentValidation;
using System.Text.RegularExpressions;

namespace MeetUpApp.ViewModels.Validation
{
    public class UserModelValidator : AbstractValidator<UserViewModel>
    {
        public UserModelValidator()
        {
            RuleFor(u => u.Username).NotEmpty()
                .MinimumLength(4)
                .MaximumLength(29)
                .Matches(/*lang=regex*/"^[A-Za-z][A-Za-z0-9_]+$",
                    RegexOptions.Compiled);

            RuleFor(p => p.Password).NotEmpty()
                .MinimumLength(7)
                .MaximumLength(32)
                .Matches(/*lang=regex*/@"[a-zA-Z0-9\!\?\*\._]+",
                    RegexOptions.Compiled);
        }
    }
}
