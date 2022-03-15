using FluentValidation;

namespace Contacts.Application.Commands.DeleteAllContactInfo
{
    public class DeleteAllContactInfoValidator : AbstractValidator<DeleteAllContactInfoCommand>
    {
        public DeleteAllContactInfoValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}
