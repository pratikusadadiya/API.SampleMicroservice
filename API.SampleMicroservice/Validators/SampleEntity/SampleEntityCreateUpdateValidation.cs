using API.SampleMicroservice.DataModels.Request;
using API.SampleMicroservice.Resources;
using FluentValidation;

namespace API.SampleMicroservice.Validators
{
    public class SampleEntityCreateUpdateValidation : AbstractValidator<SampleEntityCreateUpdateCommand>
    {
        public SampleEntityCreateUpdateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(string.Format(Messages.Required, Messages.Name))
                .MaximumLength(100).WithMessage(string.Format(Messages.MaximumLength, Messages.Name, 100));

            RuleFor(x => x.PhoneNo)
                .NotEmpty().WithMessage(string.Format(Messages.Required, Messages.Phone))
                .MaximumLength(50).WithMessage(string.Format(Messages.MaximumLength, Messages.Phone, 50));

            RuleFor(x => x.AlternatePhoneNo)
                .MaximumLength(50).WithMessage(string.Format(Messages.MaximumLength, Messages.AlternatePhoneNumber, 50));

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(string.Format(Messages.Required, Messages.Address))
                .MaximumLength(200).WithMessage(string.Format(Messages.MaximumLength, Messages.Address, 200));

            RuleFor(x => x.Comments)
                .MaximumLength(500).WithMessage(string.Format(Messages.MaximumLength, Messages.Comment, 500));
        }
    }
}
