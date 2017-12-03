namespace Employees.DtoModels
{
    using System.Collections.Generic;

    public class ManagerDto
    {
        public ManagerDto()
        {
            this.Employees = new HashSet<EmployeeDto>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeesCount { get; set; }

        public ICollection<EmployeeDto> Employees { get; set; }
    }
}