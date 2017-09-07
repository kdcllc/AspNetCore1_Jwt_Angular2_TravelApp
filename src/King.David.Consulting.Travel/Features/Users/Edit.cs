using AutoMapper;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Common.AspNetCore.Security.Password;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace King.David.Consulting.Travel.Web.Features.Users
{
    public class Edit
    {
        public class UserData
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull();
            }
        }

        public class Handler : IAsyncRequestHandler<Command, UserEnvelope>
        {
            private readonly AppDbContext _db;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(AppDbContext db, IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor)
            {
                _db = db;
                _passwordHasher = passwordHasher;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<UserEnvelope> Handle(Command message)
            {
                var currentUsername = _currentUserAccessor.GetCurrentUsername();
                var user = await _db.Users.Where(x => x.Username == currentUsername).FirstOrDefaultAsync();

                user.Username = message.User.Username ?? user.Username;
                user.Email = message.User.Email ?? user.Email;

                if (!string.IsNullOrWhiteSpace(message.User.Password))
                {
                    var salt = Guid.NewGuid().ToByteArray();
                    user.Hash = _passwordHasher.Hash(message.User.Password, salt);
                    user.Salt = salt;
                }

                await _db.SaveChangesAsync();

                return new UserEnvelope(Mapper.Map<Domain.User, User>(user));
            }
        }
    }
}
