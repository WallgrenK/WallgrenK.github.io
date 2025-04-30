using Server.Models.SeatingModels;
using Server.Models.SeatingModels.DTO;

namespace Server.Models.Interface
{
    public interface IBookingService
    {
        Task<bool> BookSeatAsync(BookSeatRequest booking);
        Task<bool> CancelBookingAsync(CancelSeatRequest request);
        Task<Seat?> GetBookingByUserIdAsync(string userId);
        Task<IEnumerable<Seat>> GetAvalibleSeatsAsync();
        Task<IEnumerable<Seat>> GetAllSeatsAsync();
    }
}
