using System;
using Microsoft.Extensions.Configuration;

namespace fugdj.Extensions
{
    public static class ConfigExtensions
    {
        public static string GetMongoConnectionString(this IConfiguration configuration)
        {
            var configValue = configuration.GetConnectionString("MongoDb");
            if (configValue != null) return configValue;

            var envValue = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
            if (envValue != null) return envValue;

            throw new Exception("Failed to find mongo connection string from app settings or env variables");
        }
        
        public static string GetYoutubeApiKey(this IConfiguration configuration)
        {
            var configValue = configuration.GetSection("Integration")["YoutubeApiKey"];
            if (configValue != null) return configValue;

            var envValue = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
            if (envValue != null) return envValue;

            throw new Exception("Failed to find youtube token from app settings or env variables");
        }
    }
}