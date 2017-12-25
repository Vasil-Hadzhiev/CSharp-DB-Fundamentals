namespace PhotoShare.Client.Utilities
{
    using PhotoShare.Client.Core;
    using PhotoShare.Models;

    public static class Checker
    {
        public static bool IsUserLoggedOn(User user)
        {
            return user.Username.Equals(Session.User.Username);
        }
    }
}