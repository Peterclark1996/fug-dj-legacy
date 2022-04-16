using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Controllers;
using fugdj.Dtos;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Services;
using fugdj.tests.Helpers;
using fugdj.tests.Mocks;
using Shouldly;
using Xunit;

namespace fugdj.tests;

public class UserTests
{
    [Fact]
    public void WhenGettingDataForAUser_UserDataIsReturned()
    {
        var userId = Common.UniqueString();
        var name = Common.UniqueString();
        var tag = new TagDbDto(0, Common.UniqueString(), Common.UniqueString());
        var tags = new List<TagDbDto> {tag};
        var mediaWithTags = new MediaWithTagsDbDto(
            new MediaDbDto(
                $"y{Common.UniqueString()}",
                Common.UniqueString(),
                30
            ),
            new List<int> {tag.Id}
        );
        var mediaList = new List<MediaWithTagsDbDto> {mediaWithTags};

        var userService = new UserService(
            new UserRepositoryMock(
                new List<UserDbDto> {new(userId, name, tags, mediaList)},
                Common.CreateNotImplementedAction<string, MediaWithTagsDbDto>(),
                Common.CreateNotImplementedAction<string, MediaUpdateDbDto>(), 
                Common.CreateNotImplementedAction<string, string>()
            ),
            new YoutubeClientMock(Common.CreateNotImplementedFunc<string, YoutubeMediaInfo>())
        );

        var userController = new UserController(userService)
        {
            ControllerContext = Common.ContextWithAuthorizedUser(userId)
        };

        var result = userController.Get().GetResponseObject<UserHttpDto>();
        result.Name.ShouldBe(name);

        var resultTag = result.Tags.ShouldHaveSingleItem();
        resultTag.Id.ShouldBe(tag.Id);
        resultTag.Name.ShouldBe(tag.Name);
        resultTag.ColourHex.ShouldBe(tag.ColourHex);

        var resultMedia = result.Media.ShouldHaveSingleItem();
        resultMedia.Code.ShouldBe(mediaWithTags.Media.Code);
        resultMedia.Name.ShouldBe(mediaWithTags.Media.Name);
        resultMedia.Player.ShouldBe(mediaWithTags.Media.Player);
        resultMedia.Tags.ShouldHaveSingleItem().ShouldBe(tag.Id);
    }

    [Fact]
    public void WhenGettingDataForAUserWithNoToken_401IsReturned()
    {
        var userService = new UserService(
            new UserRepositoryMock(
                new List<UserDbDto>(),
                Common.CreateNotImplementedAction<string, MediaWithTagsDbDto>(),
                Common.CreateNotImplementedAction<string, MediaUpdateDbDto>(), 
                Common.CreateNotImplementedAction<string, string>()
            ),
            new YoutubeClientMock(Common.CreateNotImplementedFunc<string, YoutubeMediaInfo>())
        );
        var userController = new UserController(userService);

        Should.Throw<UnauthorisedException>(
            () => userController.Get().GetResponseObject<UserHttpDto>()
        );
    }

    [Fact]
    public void WhenGettingDataForAUserThatIsNotCreated_404IsReturned()
    {
        var userService = new UserService(
            new UserRepositoryMock(
                new List<UserDbDto>(),
                Common.CreateNotImplementedAction<string, MediaWithTagsDbDto>(),
                Common.CreateNotImplementedAction<string, MediaUpdateDbDto>(), 
                Common.CreateNotImplementedAction<string, string>()
            ),
            new YoutubeClientMock(Common.CreateNotImplementedFunc<string, YoutubeMediaInfo>())
        );
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ContextWithAuthorizedUser(Common.UniqueString())
        };

        Should.Throw<ResourceNotFoundException>(
            () => userController.Get().GetResponseObject<UserHttpDto>()
        );
    }

    [Fact]
    public void WhenAddingMedia_MediaIsAddedWithCorrectData()
    {
        var userId = Common.UniqueString();
        var mediaInfo = new YoutubeMediaInfo(Common.UniqueString(), 30);
        var code = Common.UniqueString();
        var mediaHashCode = new MediaHashCodeHttpDto(Player.Youtube, code);

        string? savedMediaUserId = null;
        MediaWithTagsDbDto? savedMedia = null;

        var userService = new UserService(
            new UserRepositoryMock(new List<UserDbDto>(), (user, media) =>
            {
                savedMediaUserId = user;
                savedMedia = media;
            }, 
                Common.CreateNotImplementedAction<string, MediaUpdateDbDto>(), 
                Common.CreateNotImplementedAction<string, string>()),
            new YoutubeClientMock(mediaCode =>
            {
                if (mediaCode == code)
                {
                    return mediaInfo;
                }

                throw new Exception("Invalid media code");
            })
        );
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ContextWithAuthorizedUser(userId)
        };

        userController.AddMedia(mediaHashCode);

        savedMediaUserId.ShouldBe(userId);
        var resultMedia = savedMedia.ShouldNotBeNull();
        resultMedia.Media.HashCode.ShouldBe($"y{mediaHashCode.Code}");
        resultMedia.Media.Player.ShouldBe(mediaHashCode.Player);
        resultMedia.Media.Code.ShouldBe(mediaHashCode.Code);
        resultMedia.Media.Name.ShouldBe(mediaInfo.Name);
        resultMedia.Media.DurationSeconds.ShouldBe(mediaInfo.DurationSeconds);
        resultMedia.TagIds.ShouldBeEmpty();
    }
    
    [Fact]
    public void WhenDeletingMedia_MediaIsRemoved()
    {
        var userId = Common.UniqueString();
        var mediaHashCode = $"y{Common.UniqueString()}";

        string? deletedMediaUserId = null;
        string? deletedMedia = null;

        var userService = new UserService(
            new UserRepositoryMock(
                new List<UserDbDto>(), 
                Common.CreateNotImplementedAction<string, MediaWithTagsDbDto>(), 
                Common.CreateNotImplementedAction<string, MediaUpdateDbDto>(), 
                (user, media) =>
                {
                    deletedMediaUserId = user;
                    deletedMedia = media;
                }),
            new YoutubeClientMock(Common.CreateNotImplementedFunc<string, YoutubeMediaInfo>())
        );
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ContextWithAuthorizedUser(userId)
        };

        userController.DeleteMedia(mediaHashCode);

        deletedMediaUserId.ShouldBe(userId);
        var resultMedia = deletedMedia.ShouldNotBeNull();
        resultMedia.ShouldBe(mediaHashCode);
    }
    
    [Fact]
    public void WhenUpdatingExistingMedia_MediaIsUpdated()
    {
        var mediaName = Common.UniqueString();
        var userId = Common.UniqueString();
        var mediaHashCode = $"y{Common.UniqueString()}";
        var tags = new List<int> { 1, 5 };
        var mediaToUpdate = new MediaHttpDto(mediaName, mediaHashCode.GetPlayer(), mediaHashCode.GetCode(), tags);

        string? updatedMediaUserId = null;
        MediaUpdateDbDto? updatedMedia = null;

        var userService = new UserService(
            new UserRepositoryMock(
                new List<UserDbDto>(), 
                Common.CreateNotImplementedAction<string, MediaWithTagsDbDto>(), 
                (user, media) =>
                {
                    updatedMediaUserId = user;
                    updatedMedia = media;
                }, 
                Common.CreateNotImplementedAction<string, string>()
                ),
                new YoutubeClientMock(Common.CreateNotImplementedFunc<string, YoutubeMediaInfo>())
        );
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ContextWithAuthorizedUser(userId)
        };

        userController.UpdateMedia(mediaToUpdate);

        updatedMediaUserId.ShouldBe(userId);
        var resultMedia = updatedMedia.ShouldNotBeNull();
        resultMedia.HashCode.ShouldBe(mediaHashCode);
        resultMedia.Name.ShouldBe(mediaName);
        resultMedia.TagIds.ToList().ShouldBeEquivalentTo(tags);
    }
    
    [Fact]
    public void WhenUpdatingExistingMediaWithDuplicateTags_MediaIsUpdatedWithoutDuplicateTags()
    {
        var mediaName = Common.UniqueString();
        var userId = Common.UniqueString();
        var mediaHashCode = $"y{Common.UniqueString()}";
        var tags = new List<int> { 1, 1 };
        var mediaToUpdate = new MediaHttpDto(mediaName, mediaHashCode.GetPlayer(), mediaHashCode.GetCode(), tags);

        string? updatedMediaUserId = null;
        MediaUpdateDbDto? updatedMedia = null;

        var userService = new UserService(
            new UserRepositoryMock(
                new List<UserDbDto>(), 
                Common.CreateNotImplementedAction<string, MediaWithTagsDbDto>(), 
                (user, media) =>
                {
                    updatedMediaUserId = user;
                    updatedMedia = media;
                }, 
                Common.CreateNotImplementedAction<string, string>()
            ),
            new YoutubeClientMock(Common.CreateNotImplementedFunc<string, YoutubeMediaInfo>())
        );
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ContextWithAuthorizedUser(userId)
        };

        userController.UpdateMedia(mediaToUpdate);

        updatedMediaUserId.ShouldBe(userId);
        var resultMedia = updatedMedia.ShouldNotBeNull();
        resultMedia.HashCode.ShouldBe(mediaHashCode);
        resultMedia.Name.ShouldBe(mediaName);
        resultMedia.TagIds.ToList().ShouldBeEquivalentTo(new List<int>{ 1 });
    }
}