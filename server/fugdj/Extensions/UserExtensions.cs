using fugdj.Dtos.Db;

namespace fugdj.Extensions;

public static class UserExtensions
{
    public static int GetUnusedTagId(this UserDbDto user)
    {
        var userTagIds = user.TagList.Select(t => t.Id).ToList();
        for (var i = 0; i < userTagIds.Count + 1; i++)
        {
            if (!userTagIds.Contains(i))
            {
                return i;
            }
        }

        throw new InternalServerException();
    }
}