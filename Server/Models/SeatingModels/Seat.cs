using Server.Models.UserModels;

namespace Server.Models.SeatingModels
{
    public class Seat
    {
        public int Id { get; set; }
        public int SeatNumber { get; set; }
        public int TableId { get; set; }
        public required Table Table { get; set; }
        public string? BookedByUserId { get; set; }
        public DateTime? BookedAt { get; set; }
        public User? User { get; set; }
    }
}
