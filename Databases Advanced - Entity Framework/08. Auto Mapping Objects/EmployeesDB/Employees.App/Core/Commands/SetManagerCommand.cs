namespace Employees.App.Core.Commands
{
    using Employees.Services;

    public class SetManagerCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetManagerCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var employeeId = int.Parse(data[0]);
            var managerId = int.Parse(data[1]);

            var names = this.employeeService.SetManagerById(employeeId, managerId);

            var employeeFullName = names[0];
            var managerFullName = names[1];

            return $"{managerFullName} was succesfully set to be a manager of {employeeFullName}";
        }
    }
}