namespace fugdj.Dtos.Http;

public class UserHttpDto
{
    public string Name { get; }
    public List<TagHttpDto> Tags { get; }
    public List<MediaHttpDto> Media { get; }

    public UserHttpDto(string name, List<TagHttpDto> tags, List<MediaHttpDto> media)
    {
        Name = name;
        Tags = tags;
        Media = media;
    }
}