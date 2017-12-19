namespace FastFood.DataProcessor.Dto.Export.Xml
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoryDto
    {
        public string Name { get; set; }

        public MostPopularItemDto MostPopularItem { get; set; }
    }
}