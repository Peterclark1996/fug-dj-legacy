using System;
using fugdj.Dtos;
using fugdj.Integration;

namespace fugdj.tests.Mocks;

public class YoutubeClientMock : IYoutubeClient
{
    private readonly Func<string, YoutubeMediaInfo> _onGetMediaInfoInvoked;

    public YoutubeClientMock(Func<string, YoutubeMediaInfo> onGetMediaInfoInvoked)
    {
        _onGetMediaInfoInvoked = onGetMediaInfoInvoked;
    }

    public YoutubeMediaInfo GetMediaInfo(string mediaCode) => _onGetMediaInfoInvoked(mediaCode);
}