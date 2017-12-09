namespace Instagraph.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserPostsDto
    {
        public string Username { get; set; }

        public int MostComments { get; set; }
    }
}