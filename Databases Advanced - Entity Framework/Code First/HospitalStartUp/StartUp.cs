namespace HospitalStartUp
{
    using P01_HospitalDatabase.Data;
    using P01_HospitalDatabase.Initializer;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            DatabaseInitializer.ResetDatabase();

            //var db = new HospitalContext();

            //using (db)
            //{
            //    DatabaseInitializer.InitialSeed(db);
            //}
        }
    }
}

