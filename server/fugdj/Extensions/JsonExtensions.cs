using Newtonsoft.Json.Linq;

namespace fugdj.Extensions
{
    public static class JsonExtensions
    {
        public static JToken? FindValueWithKey(this JToken json, string key) => 
            json.ToArray().SingleOrDefault(item => item.ToString().StartsWith($"\"{key}\""))?.First;
    }
}