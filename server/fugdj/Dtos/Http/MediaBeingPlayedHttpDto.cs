namespace fugdj.Dtos.Http
{
    public class MediaBeingPlayedHttpDto
    {
        public string Name { get; }
        public Player Player { get; }
        public string Code { get; }
        public string UserId { get; }

        public MediaBeingPlayedHttpDto(string name, Player player, string code, string userId)
        {
            Name = name;
            Player = player;
            Code = code;
            UserId = userId;
        }
    }
}