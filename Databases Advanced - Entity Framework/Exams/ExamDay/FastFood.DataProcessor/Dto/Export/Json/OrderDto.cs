namespace FastFood.DataProcessor.Dto.Export.Json
{
    public class OrderDto
    {
        public string Customer { get; set; }

        public ItemDto[] Items { get; set; } = new ItemDto[0];

        public decimal TotalPrice { get; set; }
    }
}