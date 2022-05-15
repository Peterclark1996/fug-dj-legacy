namespace fugdj.Dtos.Http;

public class MediaUpdateHttpDto
{
    public string Name { get; }
    public List<int> Tags { get; }

    public MediaUpdateHttpDto(string name, List<int> tags)
    {
        Name = name;
        Tags = tags;
    }
}