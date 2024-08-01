using FluentValidation;
using PatikaMovieStore.DtoLayer.DirectorDtos;

namespace PatikaMovieStore.Validations.DirectorValidations
{
    public class UpdateDirectorValidation : AbstractValidator<UpdateDirectorDto>
    {
        public UpdateDirectorValidation()
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .GreaterThan(0);

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
