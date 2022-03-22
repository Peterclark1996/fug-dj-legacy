using System.IdentityModel.Tokens.Jwt;

namespace fugdj.Extensions;

public static class RequestExtensions
{
    public static string GetAuthorizedUserId(this HttpRequest request)
    {
        try
        {
            var token = request.Headers["Authorization"].ToString().Replace("bearer ","", StringComparison.CurrentCultureIgnoreCase);
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
        
            var userId = jwtSecurityToken.Claims.First(x => x.Type == "sub").Value;

            return userId;
        }
        catch (Exception _)
        {
            throw new UnauthorisedException();
        }
    }
}