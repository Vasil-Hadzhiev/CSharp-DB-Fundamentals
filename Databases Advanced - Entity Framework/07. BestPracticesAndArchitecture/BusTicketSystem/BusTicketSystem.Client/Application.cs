namespace BusTicketSystem.Client
{
    using BusTicketSystem.Client.Core;
    using BusTicketSystem.Client.Core.Interfaces;
    using BusTicketSystem.Data;

    public class Application
    {
        public static void Main()
        {
            ResetDatabase();
            ICommandDispatcher dispatcher = new CommandDispatcher();
            IWriter writer = new ConsoleWriter();
            IReader reader = new ConsoleReader();
            Engine engine = new Engine(dispatcher, writer, reader);
            engine.Run();
        }

        private static void ResetDatabase()
        {
            var db = new BusTicketSystemContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            System.Console.WriteLine("Created.");
        }
    }
}