namespace BusTicketSystem.Client.Core.Commands
{
    using BusTicketSystem.Client.Core.Interfaces;
    using System;    

    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}