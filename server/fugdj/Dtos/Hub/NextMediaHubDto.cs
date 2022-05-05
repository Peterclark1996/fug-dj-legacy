using fugdj.Dtos.Http;

namespace fugdj.Dtos.Hub;

public class NextMediaHubDto
{
    public MediaBeingPlayedHttpDto? JustPlayed { get; }
    public MediaBeingPlayedHttpDto? UpNext { get; }

    public NextMediaHubDto(MediaBeingPlayedHttpDto? justPlayed, MediaBeingPlayedHttpDto? upNext)
    {
        JustPlayed = justPlayed;
        UpNext = upNext;
    }
}