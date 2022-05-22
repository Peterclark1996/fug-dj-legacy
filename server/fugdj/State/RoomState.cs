using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using fugdj.Dtos;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Dtos.Hub;

namespace fugdj.State
{
    public class RoomState
    {
        public readonly Guid Id;
        public readonly string Name;

        private readonly List<ConnectedUserHubDto> _connectedUsers = new();
        
        private QueuedMedia? _currentlyPlaying;
        private DateTime? _startedPlayingAt;
        private readonly List<QueuedMedia> _mediaInQueue = new();
        private Thread? _playNextInQueueThread;

        public RoomState(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddUser(ConnectedUserHubDto user)
        {
            RemoveUser(user.Id);
            _connectedUsers.Add(user);
        }
        
        public void RemoveUser(string userId) => _connectedUsers.RemoveAll(u => u.Id == userId);

        public IEnumerable<ConnectedUserHubDto> GetUsers() => _connectedUsers;

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
            if (_currentlyPlaying != null)
            {
                mediaJustPlayed = new MediaBeingPlayedHttpDto(_currentlyPlaying.Media.Name, _currentlyPlaying.Media.Player,
                    _currentlyPlaying.Media.Code, _currentlyPlaying.UserId);
            }
        
            if (!_mediaInQueue.Any())
            {
                if (mediaJustPlayed != null)
                {
                    playMediaToRoomF.Invoke(new NextMediaHubDto(mediaJustPlayed, null));
                }
                _currentlyPlaying = null;
                _startedPlayingAt = null;
                return;
            }

            if (!HasCurrentMediaFinishedPlaying()) return;

            _currentlyPlaying = _mediaInQueue.OrderBy(m => m.TimeQueued).First();
            _mediaInQueue.Remove(_currentlyPlaying);

            _playNextInQueueThread = new Thread(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(_currentlyPlaying.Media.DurationSeconds));
                TryPlayNextInQueue(playMediaToRoomF);
            });
        
            _startedPlayingAt = DateTime.UtcNow;
            _playNextInQueueThread.Start();

            var mediaToPlay = new MediaBeingPlayedHttpDto(_currentlyPlaying.Media.Name, _currentlyPlaying.Media.Player,
                _currentlyPlaying.Media.Code, _currentlyPlaying.UserId);
            playMediaToRoomF.Invoke(new NextMediaHubDto(mediaJustPlayed, mediaToPlay));
        }

        private bool HasCurrentMediaFinishedPlaying()
        {
            if (_currentlyPlaying == null || _startedPlayingAt == null) return true;

            return _startedPlayingAt + TimeSpan.FromSeconds(_currentlyPlaying.Media.DurationSeconds) < DateTime.UtcNow;
        }
    }
}