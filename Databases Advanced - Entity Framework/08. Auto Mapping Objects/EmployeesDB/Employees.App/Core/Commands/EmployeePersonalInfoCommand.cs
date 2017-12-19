namespace Employees.App.Core.Commands
{
    using Employees.Services;
    using System.Text;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var employeeId = int.Parse(data[0]);

            var employee = this.employeeService.PersonalById(employeeId);

            var birthday = string.Empty;

            if (employee.Birthday != null)
            {
                birthday = employee.Birthday.ToString();
            }
            else
            {
                birthday = "[no birthday specified]";
            }

            var address = employee.Address ?? "[no address specified]";

            var sb = new StringBuilder();

            sb.AppendLine($"ID: {employeeId} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}");
            sb.AppendLine($"Birthday: {birthday}");
            sb.Append($"Address: {address}");

            return sb.ToString().Trim();
        }
    }
}