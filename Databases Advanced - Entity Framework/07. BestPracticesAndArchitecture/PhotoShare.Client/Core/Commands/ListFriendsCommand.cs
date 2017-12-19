namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using System;
    using System.Linq;
    using System.Text;

    public class ListFriendsCommand : Command
    {
        // PrintFriendsList <username>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var username = data[0];

            var currentUser = context.Users
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.User)
                .SingleOrDefault(u => u.Username == username);

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (currentUser.AddedAsFriendBy.Count == 0)
            {
                return "No friends for this user. :(";
            }

            var sb = new StringBuilder();

            var friendsList = currentUser.AddedAsFriendBy;

            foreach (var friendship in friendsList)
            {
                sb.AppendLine($"-{friendship.User.Username}");
            }

            return sb.ToString().Trim();
        }
    }
}