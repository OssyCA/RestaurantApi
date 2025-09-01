namespace RestaurantApi.DTO.BookingDTOs
{
    public class BookingResponseDTO
    {
        public int BookingId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime? ExpireAt { get; set; }
        public int TableId { get; set; }
    }
}
