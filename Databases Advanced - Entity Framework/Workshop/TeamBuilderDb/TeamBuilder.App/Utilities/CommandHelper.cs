﻿namespace TeamBuilder.Client.Utilities
{
    using System;
    using System.Linq;
    using TeamBuilder.Data;
    using TeamBuilder.Models;

    public static class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (var context = new TeamBuilderContext())
            {
                if (context.Users.Any(u => u.Username == username && u.IsDeleted == true))
                {
                    throw new InvalidOperationException($"There is deleted user with username {username}.");
                }
                return context.Users.Any(u => u.Username == username && u.IsDeleted == false);
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Invitations
                    .Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName && t.CreatorId == user.Id);
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(e => e.Name == eventName && e.CreatorId == user.Id);
            }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Teams
                    .Single(t => t.Name == teamName)
                    .UserTeams.Any(ut => ut.User.Username == username);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events.Any(e => e.Name == eventName);
            }
        }
    }
}