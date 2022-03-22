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
        
        return new UserHttpDto(user.Name, new List<TagHttpDto>(), user.MediaList.Select(m => new MediaHttpDto(m.Media.Name)).ToList());
    }
}