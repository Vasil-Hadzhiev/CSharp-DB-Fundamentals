namespace PhotoShare.Client.Core
{
    using PhotoShare.Client.Core.Commands;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class CommandDispatcher
    {
        private const string CommandSuffix = "Command";

        public Command DispatchCommand(string[] commandParameters)
        {
            var commandName = commandParameters[0];

            //var commandCompleteName = CultureInfo.CurrentCulture.
                //TextInfo.ToTitleCase(commandName) + CommandSuffix;

            var commandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => t.Name.Equals(commandName + CommandSuffix, StringComparison.InvariantCultureIgnoreCase));

            //var commandType = Assembly.
            //    GetExecutingAssembly().
            //    GetTypes().
            //    FirstOrDefault(t => t.Name == commandCompleteName);

            if (commandType == null)
            {
                throw new InvalidOperationException($"Command {commandName} not valid!");
            }

            var command = (Command)Activator.CreateInstance(commandType);

            return command;
        }
    }
}
