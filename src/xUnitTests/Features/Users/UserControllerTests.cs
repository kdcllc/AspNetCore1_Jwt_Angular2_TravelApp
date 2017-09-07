using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Common.Extensions;
using King.David.Consulting.Travel.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace xUnitTests.Features.Users
{
    public class UserControllerTests
    {
        [Fact]
        public void ControllerHasRouteAttributeWithCorrectTemplate()
        {
            var sut = new UserController(null, null);
            var attribute = sut.GetAttributes().OfType<RouteAttribute>().SingleOrDefault();
            attribute.ShouldBeOfType(typeof(RouteAttribute));
            attribute.Template.ShouldBe("user");
        }
        [Fact]
        public void GetCurrentHasHttpGetAttribute()
        {
            var sut = new UserController(null,null);
            var attribute = sut.GetAttributesOn(x => x.GetCurrent()).OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
        }

        [Fact]
        public async Task GetCurrentTaskWithCorrectUserName()
        {
            var model = new UserEnvelope(new User { Email = "m@m.com", Token = "1234", Username = "user1" });
            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<Details.Query>(), default(CancellationToken))).ReturnsAsync(model);

            var currentUser = new Mock<ICurrentUserAccessor>();
            currentUser.Setup(x => x.GetCurrentUsername()).Returns(model.User.Username);

            var sut = new UserController(mediator.Object, currentUser.Object);
            var result = await sut.GetCurrent();

            mediator.Verify(x=> x.Send(
                It.Is<Details.Query>(y=> y.Username == model.User.Username),default(CancellationToken)));

            //or
            result.ShouldBe(model);
        }

        [Fact]
        public void UpdateUserHasHttpPutAttribute()
        {
            var sut = new UserController(null, null);
            var attribute = sut.GetAttributesOn(x => x.UpdateUser(null)).OfType<HttpPutAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpPutAttribute));
        }

        [Fact]
        public async Task UpdateUserTaskWithCurrentUserName()
        {
            var inputModel = new Edit.UserData { Email = "m@m.com", Username = "user1", Password = "p" };
            var model = new UserEnvelope(new User { Email = "m@m.com", Token = "1234", Username = "user1" });

            var mediator = new Mock<IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<Edit.Command>(), default(CancellationToken))).ReturnsAsync(model);

            var currentUser = new Mock<ICurrentUserAccessor>();
            currentUser.Setup(x => x.GetCurrentUsername()).Returns(model.User.Username);

            var sut = new UserController(mediator.Object, currentUser.Object);
            var result = await sut.UpdateUser(new Edit.Command { User = inputModel });

            mediator.Verify(x => x.Send(
                It.Is<Edit.Command>(y => y.User.Email == model.User.Email), default(CancellationToken)),Times.Once);

            //or
            result.ShouldBe(model);
        }
    }
}
