namespace FastFood.DataProcessor.Dto.Export.Json
{
    public class EmployeeDto
    {
        public string Name { get; set; }

        public OrderDto[] Orders { get; set; } 

        public decimal TotalMade { get; set; }
    }
}
