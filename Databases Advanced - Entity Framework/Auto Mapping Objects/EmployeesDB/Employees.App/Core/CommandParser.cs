namespace Employees.App.Core
{
    using System;
    using System.Reflection;
    using System.Linq;

    using Employees.App.Core.Commands;

    public class CommandParser
    {
        private const string Suffix = "Command";

        public static ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            var fullCommandName = commandName + Suffix;

            var assembly = Assembly.GetExecutingAssembly();

            var commandTypes = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            var commandType = commandTypes
                .SingleOrDefault(t => t.Name == fullCommandName);

            if (commandType == null)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            var constructor = commandType
                .GetConstructors()
                .First();

            var constructorParameters = constructor
                .GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            var services = constructorParameters
                .Select(serviceProvider.GetService)
                .ToArray();

            var command = (ICommand)constructor.Invoke(services);

            return command;
        }
    }
}