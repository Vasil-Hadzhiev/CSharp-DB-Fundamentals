namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class AcceptFriendCommand : Command
    {
        // AcceptFriend <username1> <username2>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            if (Session.User == null)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            var receiverUsername = data[0];
            var senderUsername = data[1];

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

            if (!Checker.IsUserLoggedOn(receiver))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

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

            if (receiver.FriendsAdded.Any(fa => fa.Friend.Username == senderUsername))
            {
                throw new InvalidOperationException($"{senderUsername} is already a friend to {receiverUsername}!");
            }

            if (!sender.FriendsAdded.Any(fa => fa.Friend.Username == receiverUsername))
            {
                throw new InvalidOperationException($"{senderUsername} has not added {receiverUsername} as a friend!");
            }

            var friendship = new Friendship
            {
                User = receiver,
                Friend = sender
            };

            receiver.FriendsAdded.Add(friendship);
            context.SaveChanges();

            return $"{receiverUsername} accepted {senderUsername} as a friend.";
        }
    }
}   