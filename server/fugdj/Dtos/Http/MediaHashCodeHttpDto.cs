namespace fugdj.Dtos.Http
{
    public class MediaHashCodeHttpDto
    {
        public Player Player { get; }
        public string Code { get; }

        public MediaHashCodeHttpDto(Player player, string code)
        {
            Player = player;
            Code = code;
        }
    }
}