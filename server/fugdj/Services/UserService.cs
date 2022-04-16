using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Integration;
using fugdj.Repositories;

namespace fugdj.Services;

public interface IUserService
{
    public UserHttpDto GetUser(string userId);
    public void AddMediaForUser(string userId, MediaHashCodeHttpDto mediaToAdd);
    public void UpdateMediaForUser(string userId, MediaHttpDto mediaToUpdate);
    public void DeleteMediaForUser(string userId, string mediaToAdd);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IYoutubeClient _youtubeClient;

    public UserService(IUserRepository userRepository, IYoutubeClient youtubeClient)
    {
        _userRepository = userRepository;
        _youtubeClient = youtubeClient;
    }
    
    public UserHttpDto GetUser(string userId)
    {
        var user = _userRepository.GetUser(userId);
        if(user == null) throw new ResourceNotFoundException("User does not exist");

        var userTags = user.TagList.Select(t => 
            new TagHttpDto(t.Id, t.Name, t.ColourHex)
        ).ToList();

        var userMedia = user.MediaList.Select(m => 
            new MediaHttpDto(m.Media.Name, Player.Youtube, m.Media.HashCode[1..], m.TagIds)
        ).ToList();

        return new UserHttpDto(user.Name, userTags, userMedia);
    }

    public void AddMediaForUser(string userId, MediaHashCodeHttpDto mediaToAdd)
    {
        var hashCode = mediaToAdd.GetMediaHashCodeAsString();
        var mediaInfo = _youtubeClient.GetMediaInfo(mediaToAdd.Code);
        var media = new MediaDbDto(hashCode, mediaInfo.Name, mediaInfo.DurationSeconds);
        _userRepository.AddMediaForUser(userId, new MediaWithTagsDbDto(media, new List<int>()));
    }

    public void UpdateMediaForUser(string userId, MediaHttpDto mediaToUpdate)
    {
        var hashCode = new MediaHashCodeHttpDto(mediaToUpdate.Player, mediaToUpdate.Code).GetMediaHashCodeAsString();
        _userRepository.UpdateMediaForUser(userId, new MediaUpdateDbDto(hashCode, mediaToUpdate.Name, new HashSet<int>(mediaToUpdate.Tags)));
    }

    public void DeleteMediaForUser(string userId, string mediaToAdd)
    {
        _userRepository.DeleteMediaForUser(userId, mediaToAdd);
    }
}