namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using Utilities;
    using System.Linq;
    using System;

    public class AddTagCommand : Command
    {
        // AddTag <tag>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var tag = data[0].ValidateOrTransform();

            if (context.Tags.Any(t => t.Name == tag))
            {
                throw new ArgumentException($"Tag {tag} exists!");
            }

            var currentTag = new Tag
            {
                Name = tag
            };

            context.Tags.Add(currentTag);
            context.SaveChanges();

            return $"Tag {tag} was added successfully!";
        }
    }
}