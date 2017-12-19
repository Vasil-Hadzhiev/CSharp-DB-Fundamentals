using System;
using System.Data.SqlClient;

namespace _04.AddMinion
{
    public class StartUp
    {
        public static void Main()
        {
            var minionInfo = Console.ReadLine().Split();
            var minionName = minionInfo[1];
            var minionAge = int.Parse(minionInfo[2]);
            var town = minionInfo[3];

            var villainInfo = Console.ReadLine().Split();
            var villainName = villainInfo[1];

            var connectionString = "Server=VASIL\\SQLEXPRESS;Database=MinionsDB;Integrated Security=True;";
            var connection = new SqlConnection(connectionString);

            connection.Open();

            using (connection)
            {
                CheckIfTownExists(connection, town);
                CheckIfVillainExists(connection, villainName);
                AddMinion(connection, minionName, minionAge, town, villainName);
            }
        }

        private static void CheckIfTownExists(SqlConnection connection, string town)
        {
            var query = "SELECT Name FROM Towns WHERE Name = @town";
            var checkCommand = new SqlCommand(query, connection);
            checkCommand.Parameters.AddWithValue("@town", town);

            var foundTown = checkCommand.ExecuteScalar();

            if (foundTown == null)
            {
                var insertQuery = "INSERT INTO Towns(Name) VALUES " +
                                  "('@town')";

                var insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@town", town);

                var affectedRows = insertCommand.ExecuteNonQuery();

                if (affectedRows == 1)
                {
                    Console.WriteLine($"Town {town} was added to the database.");
                }
            }
        }

        private static void CheckIfVillainExists(SqlConnection connection, string name)
        {
            var query = "SELECT Name FROM Villains WHERE Name = @name";
            var checkCommand = new SqlCommand(query, connection);
            checkCommand.Parameters.AddWithValue("@name", name);

            var foundName = checkCommand.ExecuteScalar();

            if (foundName == null)
            {
                var insertQuery = "INSERT INTO Villains(Name) VALUES " +
                                  "('@name')";
                var insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@name", name);

                var affectedRows = insertCommand.ExecuteNonQuery();

                if (affectedRows == 1)
                {
                    Console.WriteLine($"Villain {name} added to the database.");
                }
            }
        }

        private static void AddMinion(SqlConnection connection, string minionName, int age, string town, string villainName)
        {
            var insertQuery = "INSERT INTO Minions(Name, Age, TownId) VALUES " +
                              "('@minionName', @age, (SELECT Id FROM Towns WHERE Name = @town))";
            var insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@minionName", minionName);
            insertCommand.Parameters.AddWithValue("@age", age);
            insertCommand.Parameters.AddWithValue("@town", town);

            var affectedRows = insertCommand.ExecuteNonQuery();

            if (affectedRows == 1)
            {
                var insertIdsQuery = "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES " +
                                     "((SELECT Id FROM Minions WHERE Name = @minionName), " +
                                     "(SELECT Id FROM Villains WHERE Name = @villianName))";
                var idCommand = new SqlCommand(insertIdsQuery, connection);
                idCommand.Parameters.AddWithValue("@minionName", minionName);
                idCommand.Parameters.AddWithValue("@villianName", villainName);

                var rows = idCommand.ExecuteNonQuery();

                if (rows == 1)
                {
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}");
                }
            }
        }
    }
}