using System;
using System.Linq;
using King.David.Consulting.Common.AspNetCore.Security.Token;
using King.David.Consulting.Common.Extensions;
using King.David.Consulting.Travel.Web.Features.Visits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace xUnitTests.Features.Visits
{
    public class VisitsControllerTests
    {
        [Fact]
        public void GetUserVisitsRequiredHasHttpGetAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.GetUserVisitsRequired(null,null,null))
                    .OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
            attribute.Template.ShouldBe("user/{user}/visits");
        }

        [Fact]
        public void CreateVisitRequiredHasHttpPostAttribute()
        {
            var sut = new VisitsController(null,null);
            var attribute = sut.GetAttributesOn(x => x.CreateVisitRequired(null,null))
                .OfType<HttpPostAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpPostAttribute));
            attribute.Template.ShouldBe("user/{user}/visits");
        }

        [Fact]
        public void DeleteVisitRequiredHasHttpDeleteAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.DeleteVisitRequired(null, Guid.Empty))
                .OfType<HttpDeleteAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpDeleteAttribute));
            attribute.Template.ShouldBe("user/{user}/visit/{visit}");
        }

        [Fact]
        public void GetHasHttpPostAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.Post(null))
                .OfType<HttpPostAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpPostAttribute));
            attribute.Template.ShouldBe("visits");
        }

        [Fact]
        public void PostHasHttpPostAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.Post(null))
                .OfType<HttpPostAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpPostAttribute));
            attribute.Template.ShouldBe("visits");
        }

        [Fact]
        public void PostHasAuthorizeAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.Post(null))
                .OfType<AuthorizeAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(AuthorizeAttribute));
            attribute.ActiveAuthenticationSchemes.ShouldBe(JwtIssuerOptions.Scheme);
        }

        [Fact]
        public void DeleteHasHttpPostAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.Delete(Guid.Empty))
                .OfType<HttpDeleteAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpDeleteAttribute));
            attribute.Template.ShouldBe("visits/{visit}");
        }

        [Fact]
        public void DeleteHasAuthorizeAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.Delete(Guid.Empty))
                .OfType<AuthorizeAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(AuthorizeAttribute));
            attribute.ActiveAuthenticationSchemes.ShouldBe(JwtIssuerOptions.Scheme);
        }

        [Fact]
        public void GetByAuthUserHasHttpGetAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.GetByAuthUser(null,null,null))
                .OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
            attribute.Template.ShouldBe("visits/user/{user}");
        }

        [Fact]
        public void GetByAuthUserHasAuthorizeAttribute()
        {
            var sut = new VisitsController(null, null);
            var attribute = sut.GetAttributesOn(x => x.GetByAuthUser(null,null,null))
                .OfType<AuthorizeAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(AuthorizeAttribute));
            attribute.ActiveAuthenticationSchemes.ShouldBe(JwtIssuerOptions.Scheme);
        }
    }
}
