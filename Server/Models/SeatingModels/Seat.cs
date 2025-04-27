namespace Server.Models.SeatingModels
{
    public class Seat
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int SeatNumber { get; set; }
        public bool Occupied { get; set; }
        public required Table Table { get; set; }
    }
}
