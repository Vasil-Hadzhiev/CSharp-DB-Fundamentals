namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;

    public class DeleteUserCommand : Command
    {
        // DeleteUser <username>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var username = data[0];

            var user = context.Users
                .SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (user.IsDeleted.Value)
            {
                throw new InvalidOperationException($"User {username} is already deleted!");
            }

            user.IsDeleted = true;
            context.SaveChanges();

            return $"User {username} was successfully!";
        }
    }
}