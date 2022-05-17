namespace fugdj.Dtos.Http
{
    public class TagHttpDto
    {
        public int Id { get; }
        public string Name { get; }
        public string ColourHex { get; }

        public TagHttpDto(int id, string name, string colourHex)
        {
            Id = id;
            Name = name;
            ColourHex = colourHex;
        }
    }
}