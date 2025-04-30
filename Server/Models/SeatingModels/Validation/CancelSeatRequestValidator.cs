using FluentValidation;
using Server.Models.SeatingModels.DTO;

namespace Server.Models.SeatingModels.Validation
{
    public class CancelSeatRequestValidator : AbstractValidator<CancelSeatRequest>
    {
        public CancelSeatRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty");

            RuleFor(x => x.SeatId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
