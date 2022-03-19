namespace fugdj.Dtos.Http;

public class RoomNameHttpDto
{
    public Guid Id { get; }
        
    public string Name { get; }

    public RoomNameHttpDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}