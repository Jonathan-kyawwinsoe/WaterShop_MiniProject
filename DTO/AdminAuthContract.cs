using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace water_shop.DTO
{
    public sealed record AdminLoginRequest(
        [param:Required, MinLength(2),MaxLength(50)]
        [param:Description("The unique username of administrator")]
        [param:DefaultValue("Admin")]
        string UserName,
        [param:Required, MinLength(6),MaxLength(20)]
        [param:Description("The plain-text Password of administrator")]
        [param:DefaultValue("P@ssword")]
        string Password
    );
    public sealed record AdminLoginResponse(
        [property:Description("JSON Web Token (JWT) used to authorize future requests")]
        [property:DefaultValue("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")]
        string AccessToken,
        [property: Description("Refresh token used to get a new access token or logout")]
        [property: DefaultValue("7a8b9c...")]
        string RefreshToken
    );
    public sealed record AdminLogoutRequest(
        [param: Required, MinLength(10)]
        [param: Description("The valid refresh token to be invalidated")]
        [param: DefaultValue("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")]
        string RefreshToken
    );
    public sealed record AdminLogoutResponse(
        [property: Description("The success message confirming the refresh token was revoked")]
        [property: DefaultValue("Logged out successfully. Token revoked.")]
        string Message
    );

}
