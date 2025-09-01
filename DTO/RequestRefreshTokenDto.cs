namespace RestaurantApi.DTO
{
    public class RequestRefreshTokenDto
    {
        public int Id { get; set; } 
        public required string RefreshToken { get; set; }
    }
}
