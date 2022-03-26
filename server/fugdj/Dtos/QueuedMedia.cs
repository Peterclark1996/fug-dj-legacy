using fugdj.Dtos.Http;

namespace fugdj.Dtos;

public class QueuedMedia
{
    public readonly MediaHttpDto Media;
    public readonly string UserId;
    public readonly DateTime TimeQueued;

    public QueuedMedia(MediaHttpDto media, string userId, DateTime timeQueued)
    {
        Media = media;
        UserId = userId;
        TimeQueued = timeQueued;
    }
}