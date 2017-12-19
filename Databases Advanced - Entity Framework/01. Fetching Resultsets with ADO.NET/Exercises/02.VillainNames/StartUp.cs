namespace _02.VillainNames
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            var connectionString = "Server=VASIL\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
            var connection = new SqlConnection(connectionString);

            connection.Open();

            using (connection)
            {
                var command = new SqlCommand(@"
                                SELECT v.Name, COUNT(mv.VillainId) AS [Count]
                                FROM Villains AS v
                                INNER JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
                                INNER JOIN Minions AS m ON m.Id = mv.MinionId
                                GROUP BY v.Name
                                HAVING COUNT(mv.VillainId) > 3
                                ORDER BY [Count] DESC", connection);

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No villains!");
                    return;
                }

                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} - {reader[1]}");
                }
            }
        }
    }
}