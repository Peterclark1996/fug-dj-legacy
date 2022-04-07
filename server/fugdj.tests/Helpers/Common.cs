using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;

namespace fugdj.tests.Helpers;

public static class Common
{
    public static string UniqueString() => Guid.NewGuid().ToString().Replace("-", "");
    
    public static Func<T1, T2> CreateNotImplementedFunc<T1, T2>() => _ => throw new NotImplementedException();
    
    public static Action<T1, T2> CreateNotImplementedAction<T1, T2>() => (_, _) => throw new NotImplementedException();
    
    public static T GetResponseObject<T>(this IActionResult result) where T : class
    {
        var response = result as OkObjectResult;
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(200);

        var responseObject = response.Value as T;
        responseObject.ShouldNotBeNull();
        return responseObject;
    }

    public static ControllerContext ContextWithAuthorizedUser(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("sub", userId),
            })
        };
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        var headers = new HeaderDictionary
        {
            {"Authorization", token.ShouldNotBeNull()}
        };
        
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(context => context.Request.Headers).Returns(headers);

        return new ControllerContext
        {
            HttpContext = httpContext.Object
        };
    }
}