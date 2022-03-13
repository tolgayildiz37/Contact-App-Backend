using FluentValidation;

namespace Contacts.Application.Commands.UpdateContact
{
    class UpdateContactValidator : AbstractValidator<UpdateContactCommand>
    {
        public UpdateContactValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();

            RuleFor(v => v.Name)
                .NotEmpty();
        }
    }
}
