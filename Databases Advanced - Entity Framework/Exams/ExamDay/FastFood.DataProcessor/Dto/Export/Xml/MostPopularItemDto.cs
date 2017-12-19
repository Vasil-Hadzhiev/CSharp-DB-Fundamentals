namespace FastFood.DataProcessor.Dto.Export.Xml
{
    using System.Xml.Serialization;

    [XmlType("MostPopularItem")]
    public class MostPopularItemDto
    {
        public string Name { get; set; }

        public decimal TotalMade { get; set; }

        public int TimesSold { get; set; }
    }
}