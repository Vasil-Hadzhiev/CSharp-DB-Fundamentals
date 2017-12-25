namespace BusTicketSystem.Client.Core
{ 
    using System;
    using BusTicketSystem.Client.Core.Interfaces;

    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}