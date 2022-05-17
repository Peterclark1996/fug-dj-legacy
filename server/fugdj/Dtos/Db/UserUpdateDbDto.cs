namespace fugdj.Dtos.Db;

public class UserUpdateDbDto
{
    public string Name { get; }
    
    public UserUpdateDbDto(string name)
    {
        Name = name;
    }
}