using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;

namespace fugdj.tests.Helpers;

public static class Common
{
    public static string UniqueString() => Guid.NewGuid().ToString().Replace("-", "");

    public static T GetResponseObject<T>(this IActionResult result) where T : class
    {
        var response = result as OkObjectResult;
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(200);

        var responseObject = response.Value as T;
        responseObject.ShouldNotBeNull();
        return responseObject;
    }

    public static ControllerContext ControllerContextWithAuthorizedUser(string userId)
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

    public static HubCallerContext HubContextWithAuthorizedUser(string userId)
    {
        var mockUser = new Mock<ClaimsPrincipal>();
        mockUser.Setup(u => u.Claims)
            .Returns(new List<Claim>
                {new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId)});
        
        var mockHubCallerContext = new Mock<HubCallerContext>();
        mockHubCallerContext.Setup(c => c.User).Returns(mockUser.Object);

        return mockHubCallerContext.Object;
    }
}