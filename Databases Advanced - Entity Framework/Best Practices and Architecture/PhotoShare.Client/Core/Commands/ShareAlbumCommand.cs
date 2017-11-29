namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class ShareAlbumCommand : Command
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var albumId = int.Parse(data[0]);

            var album = context.Albums
                .Include(a => a.AlbumRoles)
                .SingleOrDefault(a => a.Id == albumId);

            if (album == null)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            var username = data[1];

            var user = context.Users
                .SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var isPermissionValid = Enum.TryParse(data[2], true, out Role permissionRole);

            if (!isPermissionValid)
            {
                throw new ArgumentException("Permission must be either \"Owner\" or \"Viewer\"!");
            }

            var role = new AlbumRole
            {
                Album = album,
                User = user,
                Role = permissionRole
            };

            if (album.AlbumRoles.Any(r => r.UserId == user.Id && r.AlbumId == album.Id))
            {
                var currentRole = album.AlbumRoles
                    .Single(r => r.UserId == user.Id && r.AlbumId == album.Id).Role;

                throw new ArgumentException(
                    $"User {username} has already assigned {currentRole.ToString()} role to album {album.Name}.");
            }

            album.AlbumRoles.Add(role);
            context.SaveChanges();

            return $"Username {user.Username} added to album {album.Name} ({permissionRole.ToString()})";
        }
    }
}
