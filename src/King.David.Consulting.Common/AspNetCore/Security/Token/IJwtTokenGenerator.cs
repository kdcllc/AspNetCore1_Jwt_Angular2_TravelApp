using System.Threading.Tasks;

namespace King.David.Consulting.Common.AspNetCore.Security.Token
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}
