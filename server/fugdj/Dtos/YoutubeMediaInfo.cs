namespace fugdj.Dtos;

public class YoutubeMediaInfo
{
    public string Name { get; }
    public int DurationSeconds { get; }

    public YoutubeMediaInfo(string name, int durationSeconds)
    {
        Name = name;
        DurationSeconds = durationSeconds;
    }
}