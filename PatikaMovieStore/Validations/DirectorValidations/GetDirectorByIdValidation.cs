using FluentValidation;

namespace PatikaMovieStore.Validations.DirectorValidations
{
    public class GetDirectorByIdValidation : AbstractValidator<int>
    {
        public GetDirectorByIdValidation()
        {
            RuleFor(d => d)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
