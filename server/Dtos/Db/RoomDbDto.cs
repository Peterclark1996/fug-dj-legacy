namespace fugdj.Dtos.Db;

public class RoomDbDto
{
    public string Id { get; }
    public string Name { get; }
        
    public RoomDbDto(string id, string name)
    {
        Id = id;
        Name = name;
    }
}