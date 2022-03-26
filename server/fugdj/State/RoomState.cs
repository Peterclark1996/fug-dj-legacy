using fugdj.Dtos;
using fugdj.Dtos.Http;

namespace fugdj.State;

public class RoomState
{
    public readonly Guid Id;
    public readonly string Name;

    public QueuedMedia? CurrentlyPlaying;
    public DateTime? StartedPlayingAt;
        
    private readonly List<QueuedMedia> _mediaInQueue = new();

    public RoomState(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public void QueueMedia(MediaHttpDto media, string userId)
    {
        var queuedDatetime = 
            _mediaInQueue.SingleOrDefault(m => m.UserId == userId)?.TimeQueued ?? DateTime.UtcNow;
        var mediaToQueue = new QueuedMedia(media, userId, queuedDatetime);
            
        _mediaInQueue.RemoveAll(m => m.UserId == userId);
            
        _mediaInQueue.Add(mediaToQueue);
    }
}