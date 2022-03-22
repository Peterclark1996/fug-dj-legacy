namespace fugdj.Dtos.Http;

public class TagHttpDto
{
    public string Name { get; }
    public string ColourHex { get; }

    public TagHttpDto(string name, string colourHex)
    {
        Name = name;
        ColourHex = colourHex;
    }
}