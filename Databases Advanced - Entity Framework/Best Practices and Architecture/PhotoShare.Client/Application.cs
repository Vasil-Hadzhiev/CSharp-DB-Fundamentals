namespace PhotoShare.Client
{
    using Core;
    using Data;
    using Models;

    public class Application
    {
        public static void Main()
        {
            ResetDatabase();

            CommandDispatcher commandDispatcher = new CommandDispatcher();
            PhotoShareContext context = new PhotoShareContext();

            using (context)
            {
                Engine engine = new Engine(commandDispatcher, context);
                engine.Run();
            }
            
        }

        private static void ResetDatabase()
        {
            using (var db = new PhotoShareContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
