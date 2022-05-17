using System;
using System.Collections.Generic;
using System.Linq;
using fugdj.Controllers;
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

namespace fugdj.tests
{
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
        public void WhenUpdatingUsersUsername_UsersUsernameIsUpdated()
        {
            var userId = Common.UniqueString();

            string? updatedUserId = null;
            string? updatedUsername = null;

            var userRepo = new Mock<IUserRepository>();
            userRepo
                .Setup(u => u.UpdateUser(It.IsAny<string>(), It.IsAny<UserUpdateDbDto>()))
                .Callback<string, UserUpdateDbDto>(
                    (user, update) =>
                    {
                        updatedUserId = user;
                        updatedUsername = update.Name;
                    });
            var userService = new UserService(userRepo.Object, new Mock<IYoutubeClient>().Object);
            var userController = new UserController(userService)
            {
                ControllerContext = Common.ControllerContextWithAuthorizedUser(userId)
            };

            var usernameToUpdateTo = Common.UniqueString();
            userController.UpdateUser(new UserUpdateHttpDto(usernameToUpdateTo));

            updatedUserId.ShouldBe(userId);
            updatedUsername.ShouldBe(usernameToUpdateTo);
        }

        [Fact]
        public void WhenCreatingTagForExistingMedia_TagIsCreatedAndAddedToMedia()
        {
            var userId = Common.UniqueString();
            var mediaHashCode = $"y{Common.UniqueString()}";

            string? updatedMediaUserId = null;
            MediaUpdateDbDto? updatedMedia = null;
            TagDbDto? newTag = null;

            var userRepo = new Mock<IUserRepository>();
            userRepo
                .Setup(u => u.GetUser(userId))
                .Returns(new UserDbDto(userId, "", new List<TagDbDto> {new TagDbDto(0, "", "")},
                    new List<MediaWithTagsDbDto>
                    {
                        new(new MediaDbDto(mediaHashCode, "", 0), new List<int> {0})
                    }));
            userRepo
                .Setup(u => u.CreateTagForMedia(It.IsAny<string>(), It.IsAny<MediaUpdateDbDto>(), It.IsAny<TagDbDto>()))
                .Callback<string, MediaUpdateDbDto, TagDbDto>(
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
            resultMedia.HashCode.ShouldBe(mediaHashCode);
            resultMedia.TagIds.ToList().ShouldHaveCount(2);

            var resultTag = newTag.ShouldNotBeNull();
            resultTag.Name.ShouldBe(expectedTagName);
        }
    }
}