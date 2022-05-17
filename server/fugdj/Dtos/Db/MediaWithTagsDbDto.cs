using System.Collections.Generic;

namespace fugdj.Dtos.Db
{
    public class MediaWithTagsDbDto
    {
        public MediaDbDto Media { get; }
        public List<int> TagIds { get; }

        public MediaWithTagsDbDto(MediaDbDto media, List<int> tagIds)
        {
            Media = media;
            TagIds = tagIds;
        }
    }
}