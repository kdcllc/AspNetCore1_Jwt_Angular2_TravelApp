namespace King.David.Consulting.Common.AspNetCore.Security
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUsername();
    }
}
