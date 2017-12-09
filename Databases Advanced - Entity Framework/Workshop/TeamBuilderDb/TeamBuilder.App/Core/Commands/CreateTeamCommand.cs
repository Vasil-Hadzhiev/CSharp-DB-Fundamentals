namespace TeamBuilder.App.Core.Commands
{
    using System;

    using TeamBuilder.App.Utilities;
    using TeamBuilder.Service;

    public class CreateTeamCommand : ICommand
    {
        private readonly TeamService teamService;

        public CreateTeamCommand(TeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(params string[] args)
        {
            if (2 != args.Length && 3 != args.Length)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            if (Session.CurrentUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoginFirst);
            }

            var name = args[0];

            if (name.Length > Constants.MaxTeamNameLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamNameLengthNotValid);
            }

            var acronym = args[1];
            if (acronym.Length != Constants.TeamAcronymLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, acronym));
            }

            string description = null;

            if (args.Length == 3)
            {
                description = args[2];
            }

            if (teamService.IsTeamExisting(name))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, name));
            }

            var creatorId = Session.CurrentUser.Id;

            teamService.Create(name, acronym, description, creatorId);

            return $"Team {name} successfully created!";
        }
    }
}