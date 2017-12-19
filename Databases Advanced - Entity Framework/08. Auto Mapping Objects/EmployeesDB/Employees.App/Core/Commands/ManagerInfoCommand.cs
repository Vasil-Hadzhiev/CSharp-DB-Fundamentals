namespace Employees.App.Core.Commands
{
    using Employees.Services;
    using System.Text;

    public class ManagerInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ManagerInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var managerId = int.Parse(data[0]);

            var manager = this.employeeService.GetManagerById(managerId);

            var sb = new StringBuilder();

            sb.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.EmployeesCount}");

            foreach (var emp in manager.Employees)
            {
                sb.AppendLine($"    - {emp.FirstName} {emp.LastName} - ${emp.Salary:F2}");
            }

            return sb.ToString().Trim();
        }
    }
}