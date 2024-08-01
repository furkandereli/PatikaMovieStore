using FluentValidation;
using PatikaMovieStore.DtoLayer.OrderDtos;

namespace PatikaMovieStore.Validations.OrderValidations
{
    public class UpdateOrderValidation : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderValidation()
        {
            RuleFor(o => o.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(o => o.Price)
                .NotEmpty()
                .InclusiveBetween(100, 5000);

            RuleFor(o => o.PurchaseDate)
                .NotEmpty();

            RuleFor(o => o.CustomerId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(o => o.MovieId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
