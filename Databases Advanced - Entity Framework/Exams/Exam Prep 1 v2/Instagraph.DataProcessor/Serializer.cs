using System;

using Instagraph.Data;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using Instagraph.DataProcessor.Dto.Export;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using Microsoft.EntityFrameworkCore;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(p => p.Comments.Count == 0)
                .Select(p => new
                {
                    p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .OrderBy(p => p.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(posts, Formatting.Indented);

            return json;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts.Any(p => p.Comments
                                   .Any(c => u.Followers
                                   .Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    Username = u.Username,
                    Followers = u.Followers.Count
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Select(u => new UserPostsDto
                {
                    Username = u.Username,
                    MostComments = u.Posts.Any() ? u.Posts.Max(p => p.Comments.Count) : 0
                })
                .OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(UserPostsDto[]), new XmlRootAttribute("users"));
            serializer.Serialize(new StringWriter(sb), users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            var result = sb.ToString();

            return result;
        }
    }
}