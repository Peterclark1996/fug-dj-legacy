namespace fugdj.Dtos.Db;

public class MediaUpdateDbDto
{
    public string HashCode { get; }
    public string Name { get; }
    public List<int> TagIds { get; }

    public MediaUpdateDbDto(string hashCode, string name, List<int> tagIds)
    {
        HashCode = hashCode;
        Name = name;
        TagIds = tagIds;
    }
}