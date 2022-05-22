namespace fugdj.Dtos.Hub
{
    public class ConnectedUserHubDto
    {
        public string Id { get; }
        public string Name { get; }
        public int X { get; }
        public int Y { get; }

        public ConnectedUserHubDto(string id, string name, int x, int y)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
        }
    }
}