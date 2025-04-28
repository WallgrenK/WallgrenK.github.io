namespace Server.Models.SeatingModels.DTO
{
    public class BookingRequest
    {
    }
    public class BookSeatRequest
    {
        public int SeatId { get; set; }
        public int UserId { get; set; }
    }

    public class CancelSeatRequest
    {
        public int SeatId { get; set; }
        public int UserId { get; set; }
    }
}
