namespace _04.ChangeCasing
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

            var town = Console.ReadLine();

            var updatedTowns = new List<string>();

            connection.Open();

            using (connection)
            {
                var updateQuery = "UPDATE Towns " +
                                  "SET Name = UPPER(Name) " +
                                  "WHERE CountryId = " +
                                  "(" +
                                  "SELECT TOP 1 c.Id " +
                                  "FROM Towns AS t " +
                                  "INNER JOIN Countries AS c ON c.Id = t.CountryId " +
                                  "WHERE c.Name = @town " +
                                  ")";

                var updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@town", town);

                var affectedRows = updateCommand.ExecuteNonQuery();

                Console.WriteLine($"{affectedRows} names were affected.");

                var selectQuery = "SELECT t.Name " +
                                  "FROM Towns AS t " +
                                  "INNER JOIN Countries AS c ON c.Id = t.CountryId " +
                                  "WHERE c.Name = @town";

                var selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@town", town);

                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    updatedTowns.Add(reader[0].ToString());
                }
            }

            Console.WriteLine($"[{string.Join(", ", updatedTowns)}]");
        }
    }
}