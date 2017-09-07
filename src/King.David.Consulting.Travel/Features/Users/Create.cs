using AutoMapper;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Common.AspNetCore.Security.Password;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using King.David.Consulting.Common.AspNetCore.Security.Token;

namespace King.David.Consulting.Travel.Web.Features.Users
{
    public class Create
    {
        public class UserData
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class UserDataValidator : AbstractValidator<UserData>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
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
            private IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(AppDbContext db, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
            {
                _db = db;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<UserEnvelope> Handle(Command message)
            {
                if (await _db.Users.Where(x => x.Username == message.User.Username).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }

                if (await _db.Users.Where(x => x.Email == message.User.Email).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }

                var salt = Guid.NewGuid().ToByteArray();
                var user = new Domain.User
                {
                    Username = message.User.Username,
                    Email = message.User.Email,
                    Hash = _passwordHasher.Hash(message.User.Password, salt),
                    Salt = salt
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                string token = await _jwtTokenGenerator.CreateToken(user.Username);
                UserEnvelope envelope = new UserEnvelope(Mapper.Map<Domain.User, User>(user));
                envelope.User.Token = token;

                return envelope;
            }
        }
    }
}
