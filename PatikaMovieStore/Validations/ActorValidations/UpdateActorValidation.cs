using FluentValidation;
using PatikaMovieStore.DtoLayer.ActorDtos;

namespace PatikaMovieStore.Validations.ActorValidations
{
    public class UpdateActorValidation : AbstractValidator<UpdateActorDto>
    {
        public UpdateActorValidation()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(a => a.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(a => a.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);
        }
    }
}
