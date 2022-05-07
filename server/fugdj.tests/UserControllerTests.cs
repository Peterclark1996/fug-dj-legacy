using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Controllers;
using fugdj.Dtos;
using fugdj.Dtos.Db;
using fugdj.Dtos.Http;
using fugdj.Extensions;
using fugdj.Integration;
using fugdj.Repositories;
using fugdj.Services;
using fugdj.tests.Helpers;
using Moq;
using Shouldly;
using Xunit;

namespace fugdj.tests;

public class UserControllerTests
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

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(u => u.GetUser(userId)).Returns(new UserDbDto(userId, name, tags, mediaList));
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);

        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
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
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(u => u.GetUser(It.IsAny<string>())).Throws(new Exception());
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
        var userController = new UserController(userService);

        Should.Throw<UnauthorisedException>(
            () => userController.Get().GetResponseObject<UserHttpDto>()
        );
    }

    [Fact]
    public void WhenGettingDataForAUserThatIsNotCreated_404IsReturned()
    {
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(u => u.GetUser(It.IsAny<string>())).Returns(value: null);
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(Common.UniqueString())
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

        var userRepo = new Mock<IUserRepository>();
        userRepo
            .Setup(u => u.AddMediaForUser(It.IsAny<string>(), It.IsAny<MediaWithTagsDbDto>()))
            .Callback<string, MediaWithTagsDbDto>((user, media) =>
            {
                savedMediaUserId = user;
                savedMedia = media;
            });
        var youtubeClient = new Mock<IYoutubeClient>();
        youtubeClient.Setup(y => y.GetMediaInfo(code)).Returns(mediaInfo);
        var userService = new UserService(userRepo.Object, youtubeClient.Object);
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        userController.CreateMedia(mediaHashCode);

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

        var userRepo = new Mock<IUserRepository>();
        userRepo
            .Setup(u => u.DeleteMediaForUser(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>(
                (user, media) =>
                {
                    deletedMediaUserId = user;
                    deletedMedia = media;
                });
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
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
        var tags = new List<int> {1, 5};
        var mediaToUpdate = new MediaHttpDto(mediaName, mediaHashCode.GetPlayer(), mediaHashCode.GetCode(), tags);

        string? updatedMediaUserId = null;
        MediaUpdateDbDto? updatedMedia = null;

        var userRepo = new Mock<IUserRepository>();
        userRepo
            .Setup(u => u.UpdateMediaForUser(It.IsAny<string>(), It.IsAny<MediaUpdateDbDto>()))
            .Callback<string, MediaUpdateDbDto>(
                (user, media) =>
                {
                    updatedMediaUserId = user;
                    updatedMedia = media;
                });
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
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
        var tags = new List<int> {1, 1};
        var mediaToUpdate = new MediaHttpDto(mediaName, mediaHashCode.GetPlayer(), mediaHashCode.GetCode(), tags);

        string? updatedMediaUserId = null;
        MediaUpdateDbDto? updatedMedia = null;

        var userRepo = new Mock<IUserRepository>();
        userRepo
            .Setup(u => u.UpdateMediaForUser(It.IsAny<string>(), It.IsAny<MediaUpdateDbDto>()))
            .Callback<string, MediaUpdateDbDto>(
                (user, media) =>
                {
                    updatedMediaUserId = user;
                    updatedMedia = media;
                });
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        userController.UpdateMedia(mediaToUpdate);

        updatedMediaUserId.ShouldBe(userId);
        var resultMedia = updatedMedia.ShouldNotBeNull();
        resultMedia.HashCode.ShouldBe(mediaHashCode);
        resultMedia.Name.ShouldBe(mediaName);
        resultMedia.TagIds.ToList().ShouldBeEquivalentTo(new List<int> {1});
    }

    [Fact]
    public void WhenCreatingTagForExistingMedia_TagIsCreatedAndAddedToMedia()
    {
        var mediaName = Common.UniqueString();
        var userId = Common.UniqueString();
        var mediaHashCode = $"y{Common.UniqueString()}";

        string? updatedMediaUserId = null;
        MediaWithTagsDbDto? updatedMedia = null;
        TagDbDto? newTag = null;

        var userRepo = new Mock<IUserRepository>();
        userRepo
            .Setup(u => u.GetUser(userId))
            .Returns(new UserDbDto(userId, "", new List<TagDbDto>(), new List<MediaWithTagsDbDto>
            {
                new(new MediaDbDto(mediaHashCode, "", 0), new List<int> {0})
            }));
        userRepo
            .Setup(u => u.CreateTagForMedia(It.IsAny<string>(), It.IsAny<MediaWithTagsDbDto>(), It.IsAny<TagDbDto>()))
            .Callback<string, MediaWithTagsDbDto, TagDbDto>(
                (user, mediaUpdate, tag) =>
                {
                    updatedMediaUserId = user;
                    updatedMedia = mediaUpdate;
                    newTag = tag;
                });
        var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
        var userController = new UserController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        var mediaHashCodeObject = mediaHashCode.GetMediaHashCodeAsObject();
        var expectedTagName = Common.UniqueString();
        userController.CreateMediaTag(new CreateMediaTagHttpDto(
            new MediaHashCodeHttpDto(mediaHashCodeObject.Player, mediaHashCodeObject.Code),
            expectedTagName
        ));

        updatedMediaUserId.ShouldBe(userId);
        var resultMedia = updatedMedia.ShouldNotBeNull();
        resultMedia.TagIds.ToList().ShouldHaveCount(2);

        var resultTag = newTag.ShouldNotBeNull();
        resultTag.Name.ShouldBe(expectedTagName);
    }
}