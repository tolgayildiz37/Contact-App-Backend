using FluentValidation;

namespace Contacts.Application.Commands.DeleteContact
{
    class DeleteContactValidator : AbstractValidator<DeleteContactCommand>
    {
        public DeleteContactValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}
