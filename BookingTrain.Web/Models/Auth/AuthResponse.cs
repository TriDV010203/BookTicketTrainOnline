namespace BookingTrain.Web.Models.Auth
{
    /// <summary>Dữ liệu trả về từ API sau khi đăng nhập / đăng ký thành công.</summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
