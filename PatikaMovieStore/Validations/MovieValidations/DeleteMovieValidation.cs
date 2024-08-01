using FluentValidation;

namespace PatikaMovieStore.Validations.MovieValidations
{
    public class DeleteMovieValidation : AbstractValidator<int>
    {
        public DeleteMovieValidation()
        {
            RuleFor(m => m)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
