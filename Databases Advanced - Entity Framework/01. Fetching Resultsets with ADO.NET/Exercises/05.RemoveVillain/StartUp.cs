namespace _05.RemoveVillain
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
            var villainName = string.Empty;
            var releasedMinions = 0;

            connection.Open();

            using (connection)
            {
                var mvQuery = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                var mvCommand = new SqlCommand(mvQuery, connection);
                mvCommand.Parameters.AddWithValue("@villainId", villainId);

                releasedMinions = mvCommand.ExecuteNonQuery();

                var nameQuery = "SELECT Name FROM Villains WHERE Id = @villainId";
                var nameCommand = new SqlCommand(nameQuery, connection);
                nameCommand.Parameters.AddWithValue("@villainId", villainId);
                villainName = (string)nameCommand.ExecuteScalar();

                var vQuery = "DELETE FROM Villains WHERE Id = @villainId";
                var vCommand = new SqlCommand(vQuery, connection);
                vCommand.Parameters.AddWithValue("@villainId", villainId);
                vCommand.ExecuteNonQuery();
            }

            if (villainName == null)
            {
                Console.WriteLine("No such villain was found.");
            }
            else
            {
                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{releasedMinions} minions were released.");
            }
        }
    }
}