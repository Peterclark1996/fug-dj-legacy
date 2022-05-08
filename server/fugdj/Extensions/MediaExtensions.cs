using fugdj.Dtos.Http;

namespace fugdj.Extensions;

public static class MediaExtensions
{
    public static string GetMediaHashCodeAsString(this MediaHashCodeHttpDto media)
    {
        return media.Player switch
        {
            Player.Youtube => $"y{media.Code}",
            _ => throw new ArgumentOutOfRangeException($"Player type not recognised: {media.Player}")
        };
    }

    public static MediaHashCodeHttpDto GetMediaHashCodeAsObject(this string hashCode) =>
        new(hashCode.GetPlayer(), hashCode.GetCode());

    public static Player GetPlayer(this string hashCode)
    {
        return hashCode.First() switch
        {
            'y' => Player.Youtube,
            _ => throw new ArgumentOutOfRangeException($"Player type letter not recognised: {hashCode.First()}")
        };
    }

    public static string GetCode(this string hashCode) => hashCode[1..];
}