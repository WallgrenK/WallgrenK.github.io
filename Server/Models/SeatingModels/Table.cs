namespace Server.Models.SeatingModels
{
    public class Table
    {
        public int Id { get; set; }
        public string? TableName { get; set; }
        public int SeatsCount { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
