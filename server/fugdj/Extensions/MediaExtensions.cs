using fugdj.Dtos.Http;

namespace fugdj.Extensions;

public static class MediaExtensions
{
    public static string GetMediaHashCode(this MediaHashCodeHttpDto media)
    {
        return media.Player switch
        {
            Player.Youtube => $"y{media.Code}",
            _ => throw new ArgumentOutOfRangeException($"Player type not recognised: {media.Player}")
        };
    }
}