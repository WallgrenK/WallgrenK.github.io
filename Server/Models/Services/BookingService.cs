using Server.Models.Interface;
using Server.Models.SeatingModels;

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

        public Task<bool> BookSeatAsync(int seatId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelBookingAsync(int seatId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Table>> GetAvailableSeatsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
