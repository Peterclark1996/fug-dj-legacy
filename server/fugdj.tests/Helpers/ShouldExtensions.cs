using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Shouldly;

namespace fugdj.tests.Helpers;

public static class ShouldExtensions
{
    public static void ShouldHaveCount<T>(this IEnumerable<T> list, int count)
    {
        var shouldHaveCount = list.ToList();
        shouldHaveCount.Count.ShouldBe(count);
    }

    public static void ShouldContainEquivalent<T>(this IEnumerable<T> list, T expectedItem) where T : class =>
        list.SingleOrDefault(item => JsonSerializer.Serialize(item).Equals(JsonSerializer.Serialize(expectedItem)))
            .ShouldNotBeNull();
}