using System.Collections.Generic;

namespace fugdj.Dtos.Db
{
    public class MediaUpdateDbDto
    {
        public string HashCode { get; }
        public string Name { get; }
        public HashSet<int> TagIds { get; }

        public MediaUpdateDbDto(string hashCode, string name, HashSet<int> tagIds)
        {
            HashCode = hashCode;
            Name = name;
            TagIds = tagIds;
        }
    }
}