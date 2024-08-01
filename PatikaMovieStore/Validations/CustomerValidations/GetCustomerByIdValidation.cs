using FluentValidation;

namespace PatikaMovieStore.Validations.CustomerValidations
{
    public class GetCustomerByIdValidation : AbstractValidator<int>
    {
        public GetCustomerByIdValidation()
        {
            RuleFor(c => c)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
