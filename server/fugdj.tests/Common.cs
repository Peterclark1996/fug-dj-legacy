using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace fugdj.tests;

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

    public static void ShouldHaveCount<T>(this IEnumerable<T> list, int count)
    {
        var shouldHaveCount = list.ToList();
        shouldHaveCount.Count.ShouldBe(count);
    }

    public static void ShouldContainEquivalent<T>(this IEnumerable<T> list, T expectedItem) where T : class =>
        list.SingleOrDefault(item => JsonSerializer.Serialize(item).Equals(JsonSerializer.Serialize(expectedItem)))
            .ShouldNotBeNull();

    public static void FailTest() => true.ShouldBe(false);
}