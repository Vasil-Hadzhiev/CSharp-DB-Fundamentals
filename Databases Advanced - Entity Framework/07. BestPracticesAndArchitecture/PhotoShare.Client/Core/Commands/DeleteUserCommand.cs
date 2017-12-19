namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using PhotoShare.Client.Utilities;

    public class DeleteUserCommand : Command
    {
        // DeleteUser <username>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            if (Session.User == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var username = data[0];

            var user = context.Users
                .SingleOrDefault(u => u.Username == username);
            
            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!Checker.IsUserLoggedOn(user))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (user.IsDeleted.Value)
            {
                throw new InvalidOperationException($"User {username} is already deleted!");
            }

            user.IsDeleted = true;
            context.SaveChanges();
            Session.User = null;

            return $"User {username} was deleted successfully!";
        }
    }
}