using FluentValidation;

namespace Contacts.Application.Commands.AddContact
{
    public class AddContactValidator : AbstractValidator<AddContactCommand>
    {
        public AddContactValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty();
        }
    }
}
