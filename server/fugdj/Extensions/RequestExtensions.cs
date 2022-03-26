using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;

namespace fugdj.Extensions;

public static class RequestExtensions
{
    public static string GetAuthorizedUserId(this HttpRequest request)
    {
        try
        {
            var token = request.Headers["Authorization"].ToString()
                .Replace("bearer ", "", StringComparison.CurrentCultureIgnoreCase);
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

    public static string GetAuthorizedUserId(this HubCallerContext context)
    {
        var userId = context.User?.Claims
            .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
            ?.Value;
        if (userId == null) throw new UnauthorisedException();

        return userId;
    }
}