using System;
using fugdj.Dtos.Db;

namespace fugdj.Dtos
{
    public class QueuedMedia
    {
        public readonly MediaDbDto Media;
        public readonly string UserId;
        public readonly DateTime TimeQueued;

        public QueuedMedia(MediaDbDto media, string userId, DateTime timeQueued)
        {
            Media = media;
            UserId = userId;
            TimeQueued = timeQueued;
        }
    }
}