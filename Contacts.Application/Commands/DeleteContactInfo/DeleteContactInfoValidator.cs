using FluentValidation;

namespace Contacts.Application.Commands.DeleteContactInfo
{
    public class DeleteContactInfoValidator : AbstractValidator<DeleteContactInfoCommand>
    {
        public DeleteContactInfoValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
        }
    }
}
