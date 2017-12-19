namespace _03.MinionNames
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            var connectionString = "Server=VASIL\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
            var connection = new SqlConnection(connectionString);

            var villainId = int.Parse(Console.ReadLine());

            connection.Open();

            using (connection)
            {
                GetVillainName(connection, villainId);
                GetMinionNames(connection, villainId);
            }
        }

        private static void GetVillainName(SqlConnection connection, int villainId)
        {
            var query = "SELECT Name FROM Villains WHERE Id = @villainId";
            var villainNameCommand = new SqlCommand(query, connection);

            villainNameCommand.Parameters.AddWithValue("@villainId", villainId);

            var reader = villainNameCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Villain: {reader[0]}"); 
                }
            }
            else
            {
                Console.WriteLine($"No villain with {villainId} exists in the database.");
            }

            reader.Dispose();
        }

        private static void GetMinionNames(SqlConnection connection, int villainId)
        {
            var query = @"SELECT m.Name, m.Age
                          FROM Minions AS m
                          INNER JOIN MinionsVillains AS mv ON mv.MinionId = m.Id
                          WHERE mv.VillainId = @villainId
                          ORDER BY m.Name";

            var minionNameCommand = new SqlCommand(query, connection);

            minionNameCommand.Parameters.AddWithValue("@villainId", villainId);

            var reader = minionNameCommand.ExecuteReader();

            if (reader.HasRows)
            {
                var count = 1;

                while (reader.Read())
                {
                    Console.WriteLine($"{count}. {reader[0]} {reader[1]}");
                }
            }
            else
            {
                Console.WriteLine("(no minions)");
            }
        }
    }
}