using FluentValidation;

namespace PatikaMovieStore.Validations.OrderValidations
{
    public class GetOrderByIdValidation : AbstractValidator<int>
    {
        public GetOrderByIdValidation()
        {
            RuleFor(o => o)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
