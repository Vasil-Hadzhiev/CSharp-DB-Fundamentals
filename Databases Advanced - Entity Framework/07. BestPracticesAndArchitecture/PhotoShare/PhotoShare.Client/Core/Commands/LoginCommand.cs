namespace PhotoShare.Client.Core.Commands
{
    using System;
    using PhotoShare.Data;
    using System.Linq;

    public class LoginCommand : Command
    {
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var username = data[0];
            var password = data[1];

            var currentUser = context.Users
                .SingleOrDefault(u => u.Username.Equals(username));

            if (currentUser == null)
            {
                throw new ArgumentException("Invalid username or password!");
            }

            if (currentUser.Password != password)
            {
                throw new ArgumentException("Invalid username or password!");
            }

            if (currentUser.IsDeleted != null && currentUser.IsDeleted.Value)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            if (Session.User != null)
            {
                throw new ArgumentException("You should logout first!");
            }

            currentUser.LastTimeLoggedIn = DateTime.Now;
            context.SaveChanges();
            Session.User = currentUser; 

            return $"User {currentUser.Username} successfully logged in!";
        }
    }
}