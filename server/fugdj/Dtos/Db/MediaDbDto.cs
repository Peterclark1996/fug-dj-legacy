using fugdj.Dtos.Http;
using fugdj.Extensions;

namespace fugdj.Dtos.Db
{
    public class MediaDbDto
    {
        public string HashCode { get; }
        public string Name { get; }
        public int DurationSeconds { get; }
        public Player Player => HashCode.GetPlayer();
        public string Code => HashCode.GetCode();
        
        public MediaDbDto(string hashCode, string name, int durationSeconds)
        {
            HashCode = hashCode;
            Name = name;
            DurationSeconds = durationSeconds;
        }
    }
}