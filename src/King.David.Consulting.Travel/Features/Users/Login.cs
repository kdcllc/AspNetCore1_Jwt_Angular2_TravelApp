using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Common.AspNetCore.Security.Password;
using King.David.Consulting.Common.AspNetCore.Security.Token;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.Users
{
    public class Login
    {
        public class UserData
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class UserDataValidator : AbstractValidator<UserData>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
            }
        }

        public class Handler : IAsyncRequestHandler<Command, UserEnvelope>
        {
            private readonly AppDbContext _db;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(AppDbContext db, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
            {
                _db = db;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<UserEnvelope> Handle(Command message)
            {
                var user = await _db.Users.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync();
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                if (!user.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, user.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var mappedUser = Mapper.Map<Domain.User, User>(user); ;
                mappedUser.Token = await _jwtTokenGenerator.CreateToken(user.Username);
                return new UserEnvelope(mappedUser);
            }
        }
    }
}
