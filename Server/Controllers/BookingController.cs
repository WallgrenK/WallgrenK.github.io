using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Interface;
using Server.Models.SeatingModels;
using Server.Models.SeatingModels.DTO;

namespace Server.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IValidator<BookSeatRequest> _bookSeatValidator;
        private readonly IValidator<CancelSeatRequest> _cancelSeatValidator;

        public BookingController(IBookingService bookingService, IValidator<BookSeatRequest> bookSeatValidator, IValidator<CancelSeatRequest> cancelSeatValidator)
        {
            _bookingService = bookingService;
            _bookSeatValidator = bookSeatValidator;
            _cancelSeatValidator = cancelSeatValidator;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ProtectedPages", "booking.html");
            return PhysicalFile(filePath, "text/html");
        }

        [Authorize]
        [HttpGet("avalible")]
        public async Task<IActionResult> GetAvalibleSeats()
        {
            var seats = await _bookingService.GetAvalibleSeatsAsync();

            return Ok(seats);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSeats()
        {
            IEnumerable<Seat> seats = (await _bookingService.GetAllSeatsAsync()).ToList();

            return Ok(seats);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatByUserId(string id)
        {
            Seat? seat = await _bookingService.GetBookingByUserIdAsync(id);

            if (seat is null)
            {
                return NotFound("Booking not found.");
            }
            return Ok(seat);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BookSeat([FromBody] BookSeatRequest request)
        {
            ValidationResult validationResult = await _bookSeatValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            bool successfulBooking = await _bookingService.BookSeatAsync(request);

            if (!successfulBooking)
            {
                return BadRequest("Seat is occupied.");
            }

            return Ok("Booking was successful.");
        }

        [Authorize]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBooking([FromBody] CancelSeatRequest request)
        {
            ValidationResult validationResult = await _cancelSeatValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            bool successfulBooking = await _bookingService.CancelBookingAsync(request);

            if (!successfulBooking)
            {
                return BadRequest("Could not cancel booking.");
            }

            return Ok("Cancellation was successful.");
        }


    }
}
