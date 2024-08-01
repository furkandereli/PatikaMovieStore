using FluentValidation;
using PatikaMovieStore.DtoLayer.MovieDtos;

namespace PatikaMovieStore.Validations.MovieValidations
{
    public class UpdateMovieValidation : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieValidation()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(m => m.Title)
                .NotEmpty()
                .Length(2, 120);

            RuleFor(m => m.Price)
                .NotEmpty()
                .InclusiveBetween(100, 850);

            RuleFor(m => m.Year)
                .NotEmpty()
                .GreaterThan(1980);
        }
    }
}
