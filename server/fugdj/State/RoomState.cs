using fugdj.Dtos;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Dtos.Hub;

namespace fugdj.State;

public class RoomState
{
    public readonly Guid Id;
    public readonly string Name;

    public QueuedMedia? CurrentlyPlaying;
    public DateTime? StartedPlayingAt;

    private readonly List<QueuedMedia> _mediaInQueue = new();
    private Thread? _playNextInQueueThread;

    public RoomState(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public void QueueMedia(MediaDbDto media, string userId, Action<NextMediaHubDto> playMediaToRoomF)
    {
        var queuedDatetime =
            _mediaInQueue.SingleOrDefault(m => m.UserId == userId)?.TimeQueued ?? DateTime.UtcNow;
        var mediaToQueue = new QueuedMedia(media, userId, queuedDatetime);

        _mediaInQueue.RemoveAll(m => m.UserId == userId);

        _mediaInQueue.Add(mediaToQueue);

        TryPlayNextInQueue(playMediaToRoomF);
    }

    private void TryPlayNextInQueue(Action<NextMediaHubDto> playMediaToRoomF)
    {
        MediaBeingPlayedHttpDto? mediaJustPlayed = null;
        if (CurrentlyPlaying != null)
        {
            mediaJustPlayed = new MediaBeingPlayedHttpDto(CurrentlyPlaying.Media.Name, CurrentlyPlaying.Media.Player,
                CurrentlyPlaying.Media.Code, CurrentlyPlaying.UserId);
        }
        
        if (!_mediaInQueue.Any())
        {
            if (mediaJustPlayed != null)
            {
                playMediaToRoomF.Invoke(new NextMediaHubDto(mediaJustPlayed, null));
            }
            CurrentlyPlaying = null;
            StartedPlayingAt = null;
            return;
        }

        if (!HasCurrentMediaFinishedPlaying()) return;

        CurrentlyPlaying = _mediaInQueue.OrderBy(m => m.TimeQueued).First();
        _mediaInQueue.Remove(CurrentlyPlaying);

        _playNextInQueueThread = new Thread(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(CurrentlyPlaying.Media.DurationSeconds));
            TryPlayNextInQueue(playMediaToRoomF);
        });
        
        StartedPlayingAt = DateTime.UtcNow;
        _playNextInQueueThread.Start();

        var mediaToPlay = new MediaBeingPlayedHttpDto(CurrentlyPlaying.Media.Name, CurrentlyPlaying.Media.Player,
            CurrentlyPlaying.Media.Code, CurrentlyPlaying.UserId);
        playMediaToRoomF.Invoke(new NextMediaHubDto(mediaJustPlayed, mediaToPlay));
    }

    private bool HasCurrentMediaFinishedPlaying()
    {
        if (CurrentlyPlaying == null || StartedPlayingAt == null) return true;

        return StartedPlayingAt + TimeSpan.FromSeconds(CurrentlyPlaying.Media.DurationSeconds) < DateTime.UtcNow;
    }
}