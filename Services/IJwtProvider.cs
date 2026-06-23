namespace water_shop.Services
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(string adminId, string userName);
        string GenerateRefreshToken();
    }
}
