namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using System;
    using System.Linq;
    using System.Reflection;

    public class ModifyUserCommand : Command
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var username = data[0];
            var property = data[1];
            var newValue = data[2];

            var currentUser = context.Users
                .SingleOrDefault(u => u.Username == username);

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var userProperties = currentUser.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (!userProperties.Any(p => p.Name == property))
            {
                throw new ArgumentException($"Property {property} not supported!");
            }

            switch (property.ToLower())
            {
                case "password":
                    if (!newValue.Any(c => char.IsLower(c)) || 
                        !newValue.Any(c => char.IsDigit(c)))
                    {
                        throw new ArgumentException($"Value {newValue} not valid.{Environment.NewLine}Invalid Password");
                    }

                    currentUser.Password = newValue;
                    break;
                case "borntown":
                    var newBornTown = context.Towns
                        .SingleOrDefault(t => t.Name == newValue);

                    if (newBornTown == null)
                    {
                        throw new ArgumentException($"Value {newValue} not valid.{Environment.NewLine}Town {newValue} not found!");
                    }

                    currentUser.BornTown = newBornTown;
                    break;
                case "currenttown":
                    var newCurrentTown = context.Towns
                        .SingleOrDefault(t => t.Name == newValue);

                    if (newCurrentTown == null)
                    {
                        throw new ArgumentException($"Value {newValue} not valid.{Environment.NewLine}Town {newValue} not found!");
                    }

                    currentUser.CurrentTown = newCurrentTown;
                    break;
                default:
                    return $"Property {property} not supported!";
            }

            return $"User {username} {property} is {newValue}.";
        }
    }
}