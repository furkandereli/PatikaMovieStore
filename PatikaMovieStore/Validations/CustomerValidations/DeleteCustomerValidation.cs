using FluentValidation;

namespace PatikaMovieStore.Validations.CustomerValidations
{
    public class DeleteCustomerValidation : AbstractValidator<int>
    {
        public DeleteCustomerValidation()
        {
            RuleFor(c => c)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
