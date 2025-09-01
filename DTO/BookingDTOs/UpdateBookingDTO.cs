namespace RestaurantApi.DTO.BookingDTOs
{
    public class UpdateBookingDTO
    {
        public int TableId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTime StartAt { get; set; }
    }
}
