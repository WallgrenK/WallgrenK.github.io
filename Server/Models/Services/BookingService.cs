using Microsoft.EntityFrameworkCore;
using Server.Models.Interface;
using Server.Models.SeatingModels;
using Server.Models.SeatingModels.DTO;

namespace Server.Models.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BookingService> _logger;

        public BookingService(ILogger<BookingService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> BookSeatAsync(BookSeatRequest booking)
        {
            _logger.LogDebug("Entering BookSeatAsync");
            var seat = await _context.Seats.FindAsync(booking.SeatId);
            var user = await _context.Users.FindAsync(booking.UserId);

            if (seat == null || user == null)
            {
                _logger.LogError("Seat or user was null");
                return false;
            }

            if (seat.BookedByUserId != null)
            {
                _logger.LogError("Seat already booked");
                return false;
            }

            var existingBooking = await _context.Seats.FirstOrDefaultAsync(s => s.BookedByUserId == booking.UserId);
            if (existingBooking != null)
            {
                return false;
            }

            seat.BookedByUserId = user.Id;
            seat.BookedAt = DateTime.UtcNow;

            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelBookingAsync(CancelSeatRequest request)
        {
            var seat = await _context.Seats.FindAsync(request.SeatId);
            var user = await _context.Users.FindAsync(request.UserId);

            if (seat == null || user == null)
            {
                return false;
            }

            if (seat.BookedByUserId != user.Id)
            {
                return false;
            }

            seat.BookedAt = null;
            seat.BookedByUserId = null;

            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
        {
            return await _context.Seats.ToListAsync<Seat>();
        }

        public async Task<IEnumerable<Seat>> GetAvalibleSeatsAsync()
        {
            return await _context.Seats.Where(s => s.BookedByUserId == null).ToListAsync<Seat>();
        }

        public async Task<Seat?> GetBookingByUserIdAsync(string userId)
        {
            return await _context.Seats.FirstOrDefaultAsync(s => s.BookedByUserId == userId);
        }
    }
}
