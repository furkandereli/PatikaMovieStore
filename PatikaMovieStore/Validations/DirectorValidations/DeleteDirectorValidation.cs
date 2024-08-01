using FluentValidation;

namespace PatikaMovieStore.Validations.DirectorValidations
{
    public class DeleteDirectorValidation : AbstractValidator<int>
    {
        public DeleteDirectorValidation()
        {
            RuleFor(d => d)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
