using FluentValidation;

namespace PatikaMovieStore.Validations.ActorValidations
{
    public class GetActorByIdValidation : AbstractValidator<int>
    {
        public GetActorByIdValidation()
        {
            RuleFor(a => a)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
