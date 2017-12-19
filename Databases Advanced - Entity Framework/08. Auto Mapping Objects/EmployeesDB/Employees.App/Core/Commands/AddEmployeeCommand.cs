namespace Employees.App.Core.Commands
{
    using Employees.DtoModels;
    using Employees.Services;

    public class AddEmployeeCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public AddEmployeeCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var firstName = data[0];
            var lastName = data[1];
            var salary = decimal.Parse(data[2]);

            var employeeDto = new EmployeeDto(firstName, lastName, salary);

            this.employeeService.AddEmployee(employeeDto);

            return $"Employee {firstName} {lastName} succesfully added!";
        }
    }
}