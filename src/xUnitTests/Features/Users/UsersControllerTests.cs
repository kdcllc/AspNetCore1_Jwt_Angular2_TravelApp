using System;
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
    public class UsersControllerTests
    {
        [Fact]
        public void CreateHasHttpPostAttribute()
        {
            var sut = new UsersController(null);
            var attribute = sut.GetAttributesOn(x => x.Create(null)).OfType<HttpPostAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpPostAttribute));
        }

        [Fact]
        public void LoginHasHttpPostAttribute()
        {
            var sut = new UsersController(null);
            var attribute = sut.GetAttributesOn(x => x.Login(null)).OfType<HttpPostAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpPostAttribute));
        }
    }
}
