using FluentValidation;
using PatikaMovieStore.DtoLayer.ActorDtos;

namespace PatikaMovieStore.Validations.ActorValidations
{
    public class CreateActorValidation : AbstractValidator<CreateActorDto>
    {
        public CreateActorValidation() 
        {
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
