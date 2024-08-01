using FluentValidation;
using PatikaMovieStore.DtoLayer.DirectorDtos;

namespace PatikaMovieStore.Validations.DirectorValidations
{
    public class CreateDirectorValidation : AbstractValidator<CreateDirectorDto>
    {
        public CreateDirectorValidation()
        {
            RuleFor(d => d.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(d => d.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);
        }
    }
}
