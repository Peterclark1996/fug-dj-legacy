using System.Net.Http;
using fugdj.Dtos;
using fugdj.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace fugdj.Integration
{
    public interface IYoutubeClient
    {
        public YoutubeMediaInfo GetMediaInfo(string mediaCode);
    }

    public class YoutubeClient : IYoutubeClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _youtubeToken;

        public YoutubeClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _youtubeToken = configuration.GetSection("Integration")["YoutubeApiKey"];
        }

        public YoutubeMediaInfo GetMediaInfo(string mediaCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://www.googleapis.com/youtube/v3/videos?id={mediaCode}&part=snippet,contentDetails&key={_youtubeToken}");
            var client = _clientFactory.CreateClient();

            var response = client.Send(request);
            if (!response.IsSuccessStatusCode) throw new InternalServerException();

            var httpContent =
                JsonConvert.DeserializeObject<JToken>(response.Content.ReadAsStringAsync().Result);
            var mediaDurationString = httpContent?["items"]?.First?
                .FindValueWithKey("contentDetails")?
                .FindValueWithKey("duration")?
                .ToString();
            if (mediaDurationString == null) throw new InternalServerException();

            var mediaDuration = System.Xml.XmlConvert.ToTimeSpan(mediaDurationString);
                    
            var mediaTitle = httpContent?["items"]?.First?
                .FindValueWithKey("snippet")?
                .FindValueWithKey("title")?
                .ToString();
            if (mediaTitle == null) throw new InternalServerException();

            return new YoutubeMediaInfo(mediaTitle, (int)mediaDuration.TotalSeconds);
        }
    }
}