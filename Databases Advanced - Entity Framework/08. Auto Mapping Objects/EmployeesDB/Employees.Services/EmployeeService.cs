namespace Employees.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Employees.Data;
    using Employees.DtoModels;
    using Employees.Models;
    using System;
    using System.Linq;

    public class EmployeeService
    {
        private readonly EmployeesContext context;

        public EmployeeService(EmployeesContext context)
        {
            this.context = context;
        }

        public EmployeeDto ById(int employeeId)
        {
            var employee = this.context.Employees
                .Find(employeeId);

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);

            this.context.Employees.Add(employee);

            this.context.SaveChanges();
        }

        public string SetBirthday(int employeeId, DateTime date)
        {
            var employee = this.context.Employees
                .SingleOrDefault(e => e.Id == employeeId);

            employee.Birthday = date;

            this.context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = this.context.Employees
                .SingleOrDefault(e => e.Id == employeeId);

            employee.Address = address;

            this.context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public EmployeePersonalDto PersonalById(int employeeId)
        {
            var employee = this.context.Employees
                .SingleOrDefault(e => e.Id == employeeId);

            var employeeDto = Mapper.Map<EmployeePersonalDto>(employee);

            return employeeDto;
        }

        public ManagerDto GetManagerById(int managerId)
        {
            var manager = this.context.Employees.Find(managerId);

            var managerDto = Mapper.Map<ManagerDto>(manager);

            return managerDto;
        }

        public string[] SetManagerById(int employeeId, int managerId)
        {
            var employee = this.context.Employees
                .Find(employeeId);

            var manager = this.context.Employees
                .Find(managerId);

            employee.Manager = manager;
            manager.Employees.Add(employee);

            this.context.SaveChanges();

            var names = new string[]
            {
                $"{employee.FirstName} {employee.LastName}",
                $"{manager.FirstName} {manager.LastName}"
            };

            return names;
        }

        public EmployeeWithManagerDto[] GetEmployeesWithTheirManager(int age)
        {
            var employees = this.context
                .Employees
                .Where(emp => (DateTime.Now.Year - emp.Birthday.Value.Year) > age)
                .ProjectTo<EmployeeWithManagerDto>()
                .ToArray();

            return employees;
        }
    }
}