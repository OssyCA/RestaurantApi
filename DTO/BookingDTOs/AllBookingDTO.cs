namespace RestaurantApi.DTO.BookingDTOs
{
    public class AllBookingDTO
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime
        {
            get => StartTime.AddHours(2);
            set { }
        }
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int Amount { get; set; }
        public int TableNumber { get; set; }
    }
}
