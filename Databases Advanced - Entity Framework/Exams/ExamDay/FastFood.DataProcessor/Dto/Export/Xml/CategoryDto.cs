using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Export.Xml
{
    [XmlType("Category")]
    public class CategoryDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        public MostPopularItemDto MostPopularItem { get; set; }
    }
}