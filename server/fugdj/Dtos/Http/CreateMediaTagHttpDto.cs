namespace fugdj.Dtos.Http;

public class CreateMediaTagHttpDto
{
    public MediaHashCodeHttpDto MediaToAddTagTo { get; }

    public string TagName { get; }
    
    public CreateMediaTagHttpDto(MediaHashCodeHttpDto mediaToAddTagTo, string tagName)
    {
        MediaToAddTagTo = mediaToAddTagTo;
        TagName = tagName;
    }
}