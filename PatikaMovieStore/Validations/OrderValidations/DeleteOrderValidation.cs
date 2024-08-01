using FluentValidation;

namespace PatikaMovieStore.Validations.OrderValidations
{
    public class DeleteOrderValidation : AbstractValidator<int>
    {
        public DeleteOrderValidation()
        {
            RuleFor(o => o)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
