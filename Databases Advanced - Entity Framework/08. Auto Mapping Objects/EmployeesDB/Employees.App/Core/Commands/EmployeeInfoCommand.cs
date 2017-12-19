namespace Employees.App.Core.Commands
{
    using Employees.Services;

    public class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeeInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var employeeId = int.Parse(data[0]);

            var employee = this.employeeService.ById(employeeId);

            return $"ID: {employeeId} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}";
        }
    }
}