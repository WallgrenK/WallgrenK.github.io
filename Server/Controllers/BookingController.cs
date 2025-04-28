using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Interface;
using Server.Models.SeatingModels.DTO;

namespace Server.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
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
            var seats = await _bookingService.GetAllSeatsAsync();

            return Ok(seats);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatByUserId(int id)
        {
            var seat = await _bookingService.GetBookingByUserIdAsync(id);

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
            var successfulBooking = await _bookingService.BookSeatAsync(request);

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
            var successfulBooking = await _bookingService.CancelBookingAsync(request);

            if (!successfulBooking)
            {
                return BadRequest("Could not cancel booking.");
            }

            return Ok("Cancellation was successful.");
        }


    }
}
