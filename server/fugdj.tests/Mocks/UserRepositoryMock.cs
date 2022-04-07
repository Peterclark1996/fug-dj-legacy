using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Dtos.Db;
using fugdj.Repositories;

namespace fugdj.tests.Mocks;

public class UserRepositoryMock : IUserRepository
{
    private readonly List<UserDbDto> _users;
    private readonly Action<string, MediaWithTagsDbDto> _onAddMediaForUserInvoked;

    public UserRepositoryMock(List<UserDbDto> users, Action<string, MediaWithTagsDbDto> onAddMediaForUserInvoked)
    {
        _users = users;
        _onAddMediaForUserInvoked = onAddMediaForUserInvoked;
    }

    public UserDbDto? GetUser(string userId) => _users.SingleOrDefault(r => r.Id == userId);

    public void AddMediaForUser(string userId, MediaWithTagsDbDto mediaToAdd) =>
        _onAddMediaForUserInvoked(userId, mediaToAdd);
}