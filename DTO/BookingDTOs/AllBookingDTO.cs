namespace RestaurantApi.DTO.BookingDTOs
{
    public class AllBookingDTO
    {
        public int TableId { get; set; }
        public DateTime StartTime { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
    }
}
