namespace Employees.App.Core.Commands
{
    using Employees.Services;
    using System.Linq;
    using System.Text;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var age = int.Parse(data[0]);

            var employees = this.employeeService.GetEmployeesWithTheirManager(age);

            var sb = new StringBuilder();

            foreach (var emp in employees.OrderByDescending(e => e.Salary))
            {
                var managerName = "[no manager]";

                if (emp.ManagerName != null)
                {
                    managerName = emp.ManagerName;
                }

                sb.AppendLine($"{emp.FirstName} {emp.LastName} - ${emp.Salary} - Manager: {managerName}");
            }

            return sb.ToString().Trim();
        }
    }
}