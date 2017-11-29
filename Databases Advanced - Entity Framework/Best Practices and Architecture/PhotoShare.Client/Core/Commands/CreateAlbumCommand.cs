namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Utilities;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class CreateAlbumCommand : Command
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var username = data[0];
            var albumTitle = data[1];
            var isColorValid = Enum.TryParse(data[2], true, out Color color);
            var tags = data.Skip(3)
                .Select(t => t.ValidateOrTransform())
                .ToArray();

            var currentUser = context.Users
                .SingleOrDefault(u => u.Username == username);

            if (currentUser == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (context.Albums.Any(a => a.Name == albumTitle))
            {
                throw new ArgumentException($"Album {albumTitle} exists!");
            }

            if (!isColorValid)
            {
                throw new ArgumentException($"Color {data[2]} not found!");
            }

            //if (context.Tags.Any(t => tags.Any(tag => tag != t.Name)))
            //{
            //    throw new ArgumentException("Invalid tags!");
            //}

            if (tags.Except(context.Tags.Select(t => t.Name)).Any())
            {
                throw new ArgumentException("Invalid tags!");
            }

            var album = new Album
            {
                BackgroundColor = color,
                Name = albumTitle,
                IsPublic = true
            };

            var albumRole = new AlbumRole
            {
                User = currentUser,
                Album = album,
                Role = Role.Owner
            };

            album.AlbumRoles.Add(albumRole);
            context.Albums.Add(album);
            context.SaveChanges();

            foreach (var tag in tags)
            {
                var currentAlbum = context.Albums
                    .SingleOrDefault(a => a.Name == albumTitle);

                var currentTag = context.Tags
                    .SingleOrDefault(t => t.Name == tag);

                var albumTag = new AlbumTag
                {
                    Tag = currentTag,
                    Album = currentAlbum
                };

                currentAlbum.AlbumTags.Add(albumTag);
            }

            context.SaveChanges();

            return $"Album {album.Name} successfully created!";
        }
    }
}