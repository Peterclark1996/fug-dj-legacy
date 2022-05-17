namespace fugdj.Dtos.Db
{
    public class TagDbDto
    {
        public int Id { get; }
        public string Name { get; }
        public string ColourHex { get; }
        
        public TagDbDto(int id, string name, string colourHex)
        {
            Id = id;
            Name = name;
            ColourHex = colourHex;
        }
    }
}