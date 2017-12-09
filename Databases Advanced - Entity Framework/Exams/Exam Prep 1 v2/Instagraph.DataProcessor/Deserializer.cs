namespace Instagraph.DataProcessor
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Newtonsoft.Json;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Instagraph.Data;
    using Instagraph.Models;
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
    using Instagraph.DataProcessor.Dto.Import;
    using System.Xml.Serialization;
    using System.IO;

    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedPictures = JsonConvert.DeserializeObject<PictureDto[]>(jsonString);

            var validPictures = new List<Picture>();

            foreach (var pictureDto in deserializedPictures)
            {
                if (!IsValid(pictureDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var pathExists = validPictures.Any(p => p.Path == pictureDto.Path);

                if (pathExists)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var picture = Mapper.Map<Picture>(pictureDto);

                validPictures.Add(picture);

                sb.AppendLine($"Successfully imported Picture {pictureDto.Path}.");
            }

            context.Pictures.AddRange(validPictures);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var validUsers = new List<User>();

            foreach (var userDto in deserializedUsers)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var pictureExists = context.Pictures
                    .SingleOrDefault(p => p.Path == userDto.ProfilePicture);

                if (pictureExists == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var picture = context.Pictures
                    .Single(p => p.Path == userDto.ProfilePicture);

                var user = new User
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    ProfilePicture = picture
                };

                validUsers.Add(user);

                sb.AppendLine($"Successfully imported User {userDto.Username}.");
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var deserializedUserFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);

            var validUserFollowers = new List<UserFollower>();

            foreach (var userFollowerDto in deserializedUserFollowers)
            {
                if (!IsValid(userFollowerDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var currentUser = context.Users
                    .SingleOrDefault(u => u.Username == userFollowerDto.User);

                var currentFollower = context.Users
                    .SingleOrDefault(u => u.Username == userFollowerDto.Follower);

                var alreadyFollowed = validUserFollowers.Any(uf => uf.User.Username == userFollowerDto.User &&
                                                                   uf.Follower.Username == userFollowerDto.Follower);

                if (currentUser == null || currentFollower == null || alreadyFollowed)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var userFollower = new UserFollower
                {
                    User = currentUser,
                    Follower = currentFollower
                };

                validUserFollowers.Add(userFollower);

                sb.AppendLine($"Successfully imported Follower {currentFollower.Username} to User {currentUser.Username}.");
            }

            context.UsersFollowers.AddRange(validUserFollowers);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PostDto[]), new XmlRootAttribute("posts"));
            var deserializedPosts = (PostDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validPosts = new List<Post>();

            foreach (var postDto in deserializedPosts)
            {
                if (!IsValid(postDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var user = context.Users
                    .SingleOrDefault(u => u.Username == postDto.User);

                var picture = context.Pictures
                    .SingleOrDefault(p => p.Path == postDto.Picture);

                if (user == null || picture == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var post = new Post
                {
                    Caption = postDto.Caption,
                    User = user,
                    Picture = picture
                };

                validPosts.Add(post);

                sb.AppendLine($"Successfully imported Post {postDto.Caption}.");
            }

            context.Posts.AddRange(validPosts);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CommentDto[]), new XmlRootAttribute("comments"));
            var deserializedComments = (CommentDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validComments = new List<Comment>();

            foreach (var commentDto in deserializedComments)
            {
                if (!IsValid(commentDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var user = context.Users
                    .SingleOrDefault(u => u.Username == commentDto.User);

                var post = context.Posts
                    .SingleOrDefault(p => p.Id == commentDto.Post.Id);

                if (user == null || post == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var comment = new Comment
                {
                    Content = commentDto.Content,
                    User = user,
                    Post = post
                };

                validComments.Add(comment);

                sb.AppendLine($"Successfully imported Comment {commentDto.Content}.");
            }
          
            context.Comments.AddRange(validComments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }   

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}