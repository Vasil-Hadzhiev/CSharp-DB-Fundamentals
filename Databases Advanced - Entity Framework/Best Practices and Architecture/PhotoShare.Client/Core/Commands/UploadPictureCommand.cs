namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class UploadPictureCommand : Command
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public override string Execute(string[] data, PhotoShareContext context)
        {
            var albumName = data[0];
            var pictureTitle = data[1];
            var picturePath = data[2];

            var album = context.Albums
                .Include(a => a.Pictures)
                .SingleOrDefault(a => a.Name.Equals(albumName));

            if (album == null)
            {
                throw new ArgumentException($"Album {albumName} not found!");
            }

            var currentPicture = new Picture
            {
                Title = pictureTitle,
                Path = picturePath,
                Album = album
            };

            album.Pictures.Add(currentPicture);
            context.SaveChanges();

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}