namespace fugdj.Dtos.Http
{
    public class UserUpdateHttpDto
    {
        public string Name { get; }

        public UserUpdateHttpDto(string name)
        {
            Name = name;
        }
    }
}