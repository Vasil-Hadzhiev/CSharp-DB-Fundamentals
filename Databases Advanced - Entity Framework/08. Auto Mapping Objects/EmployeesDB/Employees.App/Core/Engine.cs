namespace Employees.App.Core
{
    using Employees.App.Core.Commands;
    using System;
    using System.Linq;

    public class Engine
    {
        private readonly IServiceProvider serviceProdiver;

        public Engine(IServiceProvider serviceProdiver)
        {
            this.serviceProdiver = serviceProdiver;
        }

        public void Run()
        {
            while (true)
            {
                var input = Console.ReadLine();
                var commandTokens = input.Split();
                var commandName = commandTokens[0];
                var commandArgs = commandTokens.Skip(1).ToArray();

                var command = CommandParser.ParseCommand(serviceProdiver, commandName);


                if (command.GetType() == typeof(ExitCommand))
                {
                    Console.WriteLine("Goodbye!");
                }

                var result = command.Execute(commandArgs);

                Console.WriteLine(result);
            }
        }
    }
}