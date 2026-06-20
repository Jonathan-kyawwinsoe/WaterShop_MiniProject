namespace water_shop.Services
{
    public interface IJwtProvider
    {
        string GenerateToken(string adminId, string userName);
    }
}
