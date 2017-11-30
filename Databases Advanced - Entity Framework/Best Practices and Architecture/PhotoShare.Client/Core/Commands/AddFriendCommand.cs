namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class AddFriendCommand : Command
    {
        // AddFriend <username1> <username2>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var senderUsername = data[0];
            var receiverUsername = data[1];

            var sender = context.Users
                .Include(u => u.FriendsAdded)
                .ThenInclude(f => f.Friend)
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.Friend)
                .SingleOrDefault(u => u.Username == senderUsername);

            if (sender == null)
            {
                throw new ArgumentException($"User {senderUsername} not found!");
            }

            var receiver = context.Users
                .Include(u => u.FriendsAdded)
                .ThenInclude(f => f.Friend)
                .Include(u => u.AddedAsFriendBy)
                .ThenInclude(f => f.Friend)
                .SingleOrDefault(u => u.Username == receiverUsername);

            if (receiver == null)
            {
                throw new ArgumentException($"User {receiverUsername} not found!");
            }

            if (sender.FriendsAdded.Any(fa => fa.Friend.Username == receiverUsername))
            {
                throw new InvalidOperationException($"{receiverUsername} is already a friend to {senderUsername}!");
            }

            var friendship = new Friendship
            {
                User = sender,
                Friend = receiver
            };

            sender.FriendsAdded.Add(friendship);
            context.SaveChanges();

            return $"Friend {receiverUsername} added to {senderUsername}.";
        }
    }
}