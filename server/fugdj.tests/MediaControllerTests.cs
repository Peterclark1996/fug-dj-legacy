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

public class MediaControllerTests
{
    [Fact]
    public void WhenAddingMedia_MediaIsAddedWithCorrectData()
    {
        var userId = Common.UniqueString();
        var mediaInfo = new YoutubeMediaInfo(Common.UniqueString(), 30);
        var code = Common.UniqueString();
        var mediaHashCode = $"y{code}";

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
        var mediaController = new MediaController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        mediaController.CreateMedia(mediaHashCode);

        savedMediaUserId.ShouldBe(userId);
        var resultMedia = savedMedia.ShouldNotBeNull();
        resultMedia.Media.HashCode.ShouldBe(mediaHashCode);
        resultMedia.Media.Player.ShouldBe(Player.Youtube);
        resultMedia.Media.Code.ShouldBe(code);
        resultMedia.Media.Name.ShouldBe(mediaInfo.Name);
        resultMedia.Media.DurationSeconds.ShouldBe(mediaInfo.DurationSeconds);
        resultMedia.TagIds.ShouldBeEmpty();
    }
    
    [Fact]
    public void WhenUpdatingExistingMedia_MediaIsUpdated()
    {
        var mediaName = Common.UniqueString();
        var userId = Common.UniqueString();
        var mediaHashCode = $"y{Common.UniqueString()}";
        var tags = new List<int> {1, 5};
        var mediaUpdate = new MediaUpdateHttpDto(mediaName, tags);

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
        var mediaController = new MediaController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        mediaController.UpdateMedia(mediaUpdate, mediaHashCode);

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
        var mediaToUpdate = new MediaUpdateHttpDto(mediaName, tags);

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
        var mediaController = new MediaController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        mediaController.UpdateMedia(mediaToUpdate, mediaHashCode);

        updatedMediaUserId.ShouldBe(userId);
        var resultMedia = updatedMedia.ShouldNotBeNull();
        resultMedia.HashCode.ShouldBe(mediaHashCode);
        resultMedia.Name.ShouldBe(mediaName);
        resultMedia.TagIds.ToList().ShouldBeEquivalentTo(new List<int> {1});
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
        var mediaController = new MediaController(userService)
        {
            ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
        };

        mediaController.DeleteMedia(mediaHashCode);

        deletedMediaUserId.ShouldBe(userId);
        var resultMedia = deletedMedia.ShouldNotBeNull();
        resultMedia.ShouldBe(mediaHashCode);
    }
}