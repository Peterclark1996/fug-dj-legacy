using System.Collections.Generic;

namespace fugdj.Dtos.Http
{
    public class MediaHttpDto
    {
        public string Name { get; }
        public Player Player { get; }
        public string Code { get; }
        public List<int> Tags { get; }

        public MediaHttpDto(string name, Player player, string code, List<int> tags)
        {
            Name = name;
            Player = player;
            Code = code;
            Tags = tags;
        }
    }

    public enum Player
    {
        Youtube
    }
}