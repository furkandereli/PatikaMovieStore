using FluentValidation;

namespace PatikaMovieStore.Validations.MovieValidations
{
    public class GetMovieByIdValidation : AbstractValidator<int>
    {
        public GetMovieByIdValidation()
        {
            RuleFor(m => m)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
