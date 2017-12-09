namespace TeamBuilder.App.Core.Commands
{
    using System;

    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;

    public class DisbandCommand : ICommand
    {
        private readonly UserService userService;
        private readonly TeamService teamService;

        public DisbandCommand(UserService userService, TeamService teamService)
        {
            this.userService = userService;
            this.teamService = teamService;
        }

        public string Execute(params string[] args)
        {
            Check.CheckLength(1, args);

            if (Session.CurrentUser == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            var teamName = args[0];

            if (!teamService.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var loggedUserId = Session.CurrentUser.Id;

            if (!userService.IsUserCreatorOfTeam(loggedUserId, teamName))
            {
                throw new ArgumentException(Constants.ErrorMessages.NotAllowed);
            }

            teamService.DisbandTeam(teamName);

            return $"{teamName} has disbanded!";
        }
    }
}