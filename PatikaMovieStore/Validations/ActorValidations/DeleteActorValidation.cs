using FluentValidation;

namespace PatikaMovieStore.Validations.ActorValidations
{
    public class DeleteActorValidation : AbstractValidator<int>
    {
        public DeleteActorValidation()
        {
            RuleFor(a => a)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
