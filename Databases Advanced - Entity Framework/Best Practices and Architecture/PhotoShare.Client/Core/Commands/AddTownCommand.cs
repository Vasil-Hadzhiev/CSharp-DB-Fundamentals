namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using System.Linq;
    using System;

    public class AddTownCommand : Command
    {
        // AddTown <townName> <countryName>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var townName = data[0];
            var countryName = data[1];

            if (context.Towns.Any(t => t.Name == townName))
            {
                throw new ArgumentException($"Town {townName} was already added!");
            }

            var town = new Town
            {
                Name = townName,
                Country = countryName
            };

            context.Towns.Add(town);
            context.SaveChanges();

            return $"Town {townName} was added successfully!";
        }
    }
}