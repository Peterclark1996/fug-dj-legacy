namespace fugdj.Dtos.Http;

public class MediaHexCodeHttpDto
{
    public Player Player { get; }
    public string Code { get; }

    public MediaHexCodeHttpDto(Player player, string code)
    {
        Player = player;
        Code = code;
    }
}