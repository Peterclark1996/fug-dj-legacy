using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Repositories;

namespace fugdj.Services;

public interface IUserService
{
    public UserHttpDto GetUser(string userId);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public UserHttpDto GetUser(string userId)
    {
        var user = _userRepository.GetUser(userId);
        if (user == null)
        {
            throw new ResourceNotFoundException("Your user does not exist");
        }

        var userTags = user.TagList.Select(t => 
            new TagHttpDto(t.Id, t.Name, t.ColourHex)
        ).ToList();

        var userMedia = user.MediaList.Select(m => 
            new MediaHttpDto(m.Media.Name, Player.Youtube, m.Media.HashCode[1..], m.TagIds)
        ).ToList();

        return new UserHttpDto(user.Name, userTags, userMedia);
    }
}