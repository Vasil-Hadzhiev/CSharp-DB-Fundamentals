namespace Employees.App.Core.Commands
{
    using System;

    public class ExitCommand : ICommand
    {
        public string Execute(params string[] data)
        {
            Environment.Exit(0);
            return null;
        }
    }
}