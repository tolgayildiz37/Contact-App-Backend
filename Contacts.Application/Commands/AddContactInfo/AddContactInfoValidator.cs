using FluentValidation;

namespace Contacts.Application.Commands.AddContactInfo
{
    public class AddContactInfoValidator : AbstractValidator<AddContactInfoCommand>
    {
        public AddContactInfoValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}
