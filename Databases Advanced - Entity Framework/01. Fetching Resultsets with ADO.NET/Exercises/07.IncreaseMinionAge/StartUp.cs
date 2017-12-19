namespace _07.IncreaseMinionAge
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var connectionString = "Server=VASIL\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
            var connection = new SqlConnection(connectionString);

            var ids = Console.ReadLine().
                Split().
                Select(int.Parse).
                ToArray();

            connection.Open();

            using (connection)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    var currentMinionId = ids[i];

                    var increaseAgeQuery = "UPDATE Minions " +
                                           "SET Age = Age + 1 " +
                                           "WHERE Id = @currentMinionId";

                    var increaseAgeCommand = new SqlCommand(increaseAgeQuery, connection);
                    increaseAgeCommand.Parameters.AddWithValue("@currentMinionId", currentMinionId);

                    increaseAgeCommand.ExecuteNonQuery();

                    var updateNameQuery = "UPDATE Minions " +
                                          "SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)) " +
                                          "WHERE Id = @currentMinionId";

                    var updateNameCommand = new SqlCommand(updateNameQuery, connection);
                    updateNameCommand.Parameters.AddWithValue("@currentMinionId", currentMinionId);
                }

                var selectQuery = "SELECT Name, Age FROM Minions";
                var selectCommand = new SqlCommand(selectQuery, connection);
                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0].ToString()} {reader[1].ToString()}");
                }             
            }
        }
    }
}