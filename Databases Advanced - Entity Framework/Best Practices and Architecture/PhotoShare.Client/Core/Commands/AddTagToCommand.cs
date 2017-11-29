namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class AddTagToCommand : Command
    {
        // AddTagTo <albumName> <tag>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var albumName = data[0];
            var tag = data[1].ValidateOrTransform();

            if (!context.Tags.Any(t => t.Name == tag))
            {
                throw new ArgumentException("Either tag or album does not exist!");
            }

            if (!context.Albums.Any(a => a.Name == albumName))
            {
                throw new ArgumentException("Either tag or album does not exist!");
            }

            var currentAlbum = context.Albums
                .SingleOrDefault(a => a.Name == albumName);

            var currentTag = context.Tags
                .SingleOrDefault(t => t.Name == tag);


            var currentAlbumTag = new AlbumTag
            {
                Album = currentAlbum,
                Tag = currentTag
            };

            currentAlbum.AlbumTags.Add(currentAlbumTag);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new ArgumentException($"Tag {tag} has already been added to {albumName}!");
            }

            return $"Tag {currentTag.Name} added to {currentAlbum.Name}!";
        }
    }
}