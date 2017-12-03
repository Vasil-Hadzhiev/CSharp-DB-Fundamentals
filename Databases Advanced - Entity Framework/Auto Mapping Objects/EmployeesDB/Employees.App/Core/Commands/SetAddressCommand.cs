namespace Employees.App.Core.Commands
{
    using Employees.Services;
    using System.Linq;

    public class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var employeeId = int.Parse(data[0]);
            var address = string.Join(" ", data.Skip(1));

            var employeeName = this.employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address was updated succesfully!";
        }
    }
}