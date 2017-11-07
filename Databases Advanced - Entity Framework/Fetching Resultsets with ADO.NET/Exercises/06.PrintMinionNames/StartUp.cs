namespace _06.PrintMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            var connectionString = "Server=VASIL\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
            var connection = new SqlConnection(connectionString);
            var minions = new List<string>();

            connection.Open();

            using (connection)
            {
                var selectQuery = "SELECT Name FROM Minions";
                var selectCommand = new SqlCommand(selectQuery, connection);
                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    minions.Add(reader[0].ToString());
                }
            }

            var counter = 1;

            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine($"{minions[-1 + counter]}");
                Console.WriteLine($"{minions[minions.Count - counter]}");
                counter++;
            }
        }
    }
}
