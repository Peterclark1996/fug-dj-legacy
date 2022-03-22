namespace fugdj.Dtos.Db;

public class MediaDbDto
{
    public string HashCode { get; }
    public string Name { get; }
    public int DurationSeconds { get; }
        
    public MediaDbDto(string hashCode, string name, int durationSeconds)
    {
        HashCode = hashCode;
        Name = name;
        DurationSeconds = durationSeconds;
    }
}