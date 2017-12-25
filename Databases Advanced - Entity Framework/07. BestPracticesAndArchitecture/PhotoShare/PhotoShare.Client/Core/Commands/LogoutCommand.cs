namespace PhotoShare.Client.Core.Commands
{
    using System;
    using PhotoShare.Data;

    public class LogoutCommand : Command
    {
        public override string Execute(string[] data, PhotoShareContext context)
        {
            if (Session.User == null)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            Session.User = null;

            return $"User {Session.User.Username} successfully logged out!";
        }
    }
}