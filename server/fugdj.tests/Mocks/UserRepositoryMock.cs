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
    private readonly Action<string, MediaUpdateDbDto> _onUpdateMediaForUserInvoked;
    private readonly Action<string, string> _onDeleteMediaForUserInvoked;

    public UserRepositoryMock(
        List<UserDbDto> users,
        Action<string, MediaWithTagsDbDto> onAddMediaForUserInvoked,
        Action<string, MediaUpdateDbDto> onUpdateMediaForUserInvoked,
        Action<string, string> onDeleteMediaForUserInvoked
    )
    {
        _users = users;
        _onAddMediaForUserInvoked = onAddMediaForUserInvoked;
        _onUpdateMediaForUserInvoked = onUpdateMediaForUserInvoked;
        _onDeleteMediaForUserInvoked = onDeleteMediaForUserInvoked;
    }

    public UserDbDto? GetUser(string userId) => _users.SingleOrDefault(r => r.Id == userId);

    public void AddMediaForUser(string userId, MediaWithTagsDbDto mediaToAdd) =>
        _onAddMediaForUserInvoked(userId, mediaToAdd);

    public void UpdateMediaForUser(string userId, MediaUpdateDbDto mediaToUpdate) =>
        _onUpdateMediaForUserInvoked(userId, mediaToUpdate);

    public void DeleteMediaForUser(string userId, string hashCode) =>
        _onDeleteMediaForUserInvoked(userId, hashCode);
}