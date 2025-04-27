using Server.Models.SeatingModels;

namespace Server.Models.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<Table>> GetAvailableSeatsAsync();

        Task<bool> BookSeatAsync(int seatId, int userId);
        Task<bool> CancelBookingAsync(int seatId, int userId);
    }
}
