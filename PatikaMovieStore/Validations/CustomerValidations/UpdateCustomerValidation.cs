using FluentValidation;
using PatikaMovieStore.DtoLayer.CustomerDtos;

namespace PatikaMovieStore.Validations.CustomerValidations
{
    public class UpdateCustomerValidation : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(c => c.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);

            RuleFor(c => c.Email)
                .NotEmpty()
                .Length(10, 100);

            RuleFor(c => c.Password)
                .NotEmpty()
                .Length(8, 16);
        }
    }
}
