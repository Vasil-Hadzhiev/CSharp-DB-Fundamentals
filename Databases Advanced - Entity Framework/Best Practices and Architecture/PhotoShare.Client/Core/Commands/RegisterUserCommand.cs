namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Models;
    using Data;
    using System.Linq;

    public class RegisterUserCommand : Command
    {
        // RegisterUser <username> <password> <repeat-password> <email>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            if (Session.User != null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var username = data[0];
            var password = data[1];
            var repeatPassword = data[2];
            var email = data[3];

            if (context.Users.Any(u => u.Username == username))
            {
                throw new InvalidOperationException($"Username {username} is already taken!");
            }

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            var user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();

            return $"User {user.Username} was registered successfully!";
        }
    }
}