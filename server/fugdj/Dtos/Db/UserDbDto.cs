using System.Collections.Generic;

namespace fugdj.Dtos.Db
{
    public class UserDbDto
    {
        public string Id { get; }
        public string Name { get; }
        public List<TagDbDto> TagList { get; }
        public List<MediaWithTagsDbDto> MediaList { get; }
        
        public UserDbDto(string id, string name, List<TagDbDto> tagList, List<MediaWithTagsDbDto> mediaList)
        {
            Id = id;
            Name = name;
            TagList = tagList;
            MediaList = mediaList;
        }
    }
}