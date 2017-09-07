namespace King.David.Consulting.Common.AspNetCore.Security.Password
{
    public interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }
}
