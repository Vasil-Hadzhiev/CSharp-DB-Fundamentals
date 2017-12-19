namespace Employees.App.Core.Commands
{
    using Employees.Services;
    using System;

    public class SetBirthdayCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetBirthdayCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var employeeId = int.Parse(data[0]);
            var date = DateTime.ParseExact(data[1], "dd-MM-yyyy", null);

            var employeeName = this.employeeService.SetBirthday(employeeId, date);

            return $"{employeeName}'s birthday was updated succesfully!";
        }
    }
}
