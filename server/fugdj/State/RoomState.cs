using fugdj.Dtos;
using fugdj.Dtos.Db;

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

    public void QueueMedia(MediaDbDto media, string userId, Action playMediaToRoomF)
    {
        var queuedDatetime = 
            _mediaInQueue.SingleOrDefault(m => m.UserId == userId)?.TimeQueued ?? DateTime.UtcNow;
        var mediaToQueue = new QueuedMedia(media, userId, queuedDatetime);
            
        _mediaInQueue.RemoveAll(m => m.UserId == userId);
            
        _mediaInQueue.Add(mediaToQueue);
        
        TryPlayNextInQueue(playMediaToRoomF);
    }

    private void TryPlayNextInQueue(Action playMediaToRoomF)
    {
        if (!_mediaInQueue.Any())
        {
            CurrentlyPlaying = null;
            StartedPlayingAt = null;
            return;
        }

        if (!HasCurrentMediaFinishedPlaying()) return;

        CurrentlyPlaying = _mediaInQueue.OrderBy(m => m.TimeQueued).First();
        StartedPlayingAt = DateTime.UtcNow;
        playMediaToRoomF.Invoke();
        _mediaInQueue.Remove(CurrentlyPlaying);
    }

    private bool HasCurrentMediaFinishedPlaying()
    {
        if (CurrentlyPlaying == null || StartedPlayingAt == null) return true;

        return StartedPlayingAt + TimeSpan.FromSeconds(CurrentlyPlaying.Media.DurationSeconds) < DateTime.UtcNow;
    }
}