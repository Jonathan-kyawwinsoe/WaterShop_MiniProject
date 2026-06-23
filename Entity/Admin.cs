namespace water_shop.Entity
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? CreadeddAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; } = DateTime.Now;
    }
}
