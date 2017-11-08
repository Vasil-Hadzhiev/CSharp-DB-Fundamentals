using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Linq;

namespace P02_DatabaseFirst
{
    public class StartUp
    {
        public static void Main()
        {
            var db = new SoftUniContext();

            using (db)
            {
                //Problem 3
                //EmployeeFullInofmartion(db);

                //Problem 4
                EmployeesSalaryOver50000(db);

                //Problem 5
                //EmployeesFromRnD(db);

                //Problem 6
                //AddNewAddressAndUpdateEmployee(db);

                
            }

        }

        private static void AddNewAddressAndUpdateEmployee(SoftUniContext db)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            db.Addresses.Add(address);

            var employee = db.Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();

            employee.Address = address;

            db.SaveChanges();

            var employees = db.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    AddressText = e.Address.AddressText
                })
                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine(emp.AddressText);
            }
        }

        private static void EmployeesFromRnD(SoftUniContext db)
        {
            var employees = db
                                .Employees
                                .Where(e => e.Department.Name == "Research and Development")
                                .OrderBy(e => e.Salary)
                                .ThenByDescending(e => e.FirstName)
                                .Select(e => new
                                {
                                    Name = $"{e.FirstName} {e.LastName}",
                                    e.Salary,
                                    DepartmentName = e.Department.Name
                                })
                                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Name} from {emp.DepartmentName} - ${emp.Salary:f2}");
            }
        }

        private static void EmployeesSalaryOver50000(SoftUniContext db)
        {
            var employees = db
                .Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName
                })
                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine(emp.FirstName);
            }
        }

        private static void EmployeeFullInofmartion(SoftUniContext db)
        {

            var employees = db
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    Name = $"{e.FirstName} {e.LastName} {e.MiddleName}",
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Name} {emp.JobTitle} {emp.Salary:f2}");
            }
        }
    }
}