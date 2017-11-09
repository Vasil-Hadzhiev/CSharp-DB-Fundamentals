using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Collections.Generic;
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
                //EmployeesSalaryOver50000(db);

                //Problem 5
                //EmployeesFromRnD(db);

                //Problem 6
                //AddNewAddressAndUpdateEmployee(db);

                //Problem 7
                //EmployeesAndProjects(db);

                //Problem 8
                //AddressesByTown(db);

                //Problem 9
                //Employee147(db);

                //Problem 10
                //DepartmentsWithMoreThan5Employees(db);

                //Problem 11
                //FindLatest10Projects(db);

                //Problem 12
                //IncreaseSalaries(db);

                //Problem 13
                //EmployeesStartingWith(db);

                //Problem 14
                //DeleteProject(db);

                //Problem 15
                //RemoveTown(db);
            }
        }

        private static void RemoveTown(SoftUniContext db)
        {
            var townToDelete = Console.ReadLine();

            List<Address> addressesToRemove = db.Addresses
                .Where(a => a.Town.Name == townToDelete)
                .ToList();

            var employees = db.Employees.ToList();

            foreach (var emp in employees)
            {
                if (addressesToRemove.Contains(emp.Address))
                {
                    emp.Address = null;
                }
            }

            db.Addresses.RemoveRange(addressesToRemove);

            var town = db.Towns
                .SingleOrDefault(t => t.Name == townToDelete);

            db.Towns.Remove(town);
            db.SaveChanges();

            var removedAddressesCount = addressesToRemove.Count;

            if (removedAddressesCount > 1)
            {
                Console.WriteLine($"{removedAddressesCount} addresses in {townToDelete} were deleted");
            }
            else
            {
                Console.WriteLine($"{removedAddressesCount} address in {townToDelete} was deleted");
            }
        }

        private static void DeleteProject(SoftUniContext db)
        {
            var projectId = 2;

            var empProjects = db.EmployeesProjects
                .Where(ep => ep.ProjectId == projectId)
                .ToList();

            foreach (var ep in empProjects)
            {
                db.EmployeesProjects.Remove(ep);
            }

            var project = db.Projects.Find(2);

            db.Projects.Remove(project);

            db.SaveChanges();

            var projects = db.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList();

            foreach (var proj in projects)
            {
                Console.WriteLine(proj);
            }
        }

        private static void EmployeesStartingWith(SoftUniContext db)
        {
            var employees = db.Employees
                                .Where(e => e.FirstName.StartsWith("Sa"))
                                .OrderBy(e => e.FirstName)
                                .ThenBy(e => e.LastName)
                                .Select(e => new
                                {
                                    Name = $"{e.FirstName} {e.LastName}",
                                    e.JobTitle,
                                    e.Salary
                                })
                                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Name} - {emp.JobTitle} - (${emp.Salary:f2})");
            }
        }

        private static void IncreaseSalaries(SoftUniContext db)
        {
            var employees = db.Employees
                                .Where(e =>
                                    e.Department.Name == "Engineering" ||
                                    e.Department.Name == "Tool Design" ||
                                    e.Department.Name == "Marketing" ||
                                    e.Department.Name == "Information Services")
                                .OrderBy(e => e.FirstName)
                                .ThenBy(e => e.LastName)
                                .ToList();

            foreach (var emp in employees)
            {
                emp.Salary += emp.Salary * 12 / 100;
                Console.WriteLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            db.SaveChanges();
        }

        private static void FindLatest10Projects(SoftUniContext db)
        {
            var projects = db.Projects
                                .OrderByDescending(p => p.StartDate)
                                .Select(p => new
                                {
                                    p.Name,
                                    p.Description,
                                    p.StartDate
                                })
                                .Take(10)
                                .OrderBy(p => p.Name)
                                .ToList();

            foreach (var project in projects)
            {
                Console.WriteLine(project.Name);
                Console.WriteLine(project.Description);
                Console.WriteLine(project.StartDate);
            }
        }

        private static void DepartmentsWithMoreThan5Employees(SoftUniContext db)
        {
            var departments = db.Departments
                                .Where(d => d.Employees.Count > 5)
                                .OrderBy(d => d.Employees.Count)
                                .ThenBy(d => d.Name)
                                .Select(d => new
                                {
                                    d.Name,
                                    ManagerName = $"{d.Manager.FirstName} {d.Manager.LastName}",
                                    Employees = d.Employees.Select(e => new
                                    {
                                        e.FirstName,
                                        e.LastName,
                                        e.JobTitle
                                    })
                                })
                                .ToList();

            foreach (var dep in departments)
            {
                Console.WriteLine($"{dep.Name} - {dep.ManagerName}");

                foreach (var emp in dep.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName))
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }

                Console.WriteLine("----------");
            }
        }

        private static void Employee147(SoftUniContext db)
        {
            var employee = db.Employees
                                .Select(e => new
                                {
                                    e.EmployeeId,
                                    Name = $"{e.FirstName} {e.LastName}",
                                    e.JobTitle,
                                    Projects = e.EmployeeProjects.Select(ep => new
                                    {
                                        Name = ep.Project.Name
                                    })
                                })
                                .SingleOrDefault(e => e.EmployeeId == 147);

            Console.WriteLine($"{employee.Name} - {employee.JobTitle}");

            foreach (var project in employee.Projects
                .OrderBy(p => p.Name))
            {
                Console.WriteLine(project.Name);
            }
        }

        private static void AddressesByTown(SoftUniContext db)
        {
            var addresses = db.Addresses
                                .OrderByDescending(a => a.Employees.Count())
                                .ThenBy(a => a.Town.Name)
                                .ThenBy(a => a.AddressText)
                                .Take(10)
                                .Select(a => new
                                {
                                    a.AddressText,
                                    Town = a.Town.Name,
                                    EmpCount = a.Employees.Count()
                                })
                                .ToList();

            foreach (var adr in addresses)
            {
                Console.WriteLine($"{adr.AddressText}, {adr.Town} - {adr.EmpCount} employees");
            }
        }

        private static void EmployeesAndProjects(SoftUniContext db)
        {
            var employees = db.Employees
                                .Where(e =>
                                    e.EmployeeProjects.Any(ep =>
                                    ep.Project.StartDate.Year >= 2001 &&
                                    ep.Project.StartDate.Year <= 2003
                                ))
                                .Select(e => new
                                {
                                    Name = $"{e.FirstName} {e.LastName}",
                                    ManagerName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                                    Projects = e.EmployeeProjects.
                                        Select(ep => new
                                        {
                                            ep.Project.Name,
                                            ep.Project.StartDate,
                                            ep.Project.EndDate
                                        })
                                })
                                .Take(30)
                                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Name} - Manager: {emp.ManagerName}");

                foreach (var p in emp.Projects)
                {
                    if (p.EndDate == null)
                    {
                        Console.WriteLine($"--{p.Name} - {p.StartDate} - not finished");
                    }
                    else
                    {
                        Console.WriteLine($"--{p.Name} - {p.StartDate} - {p.EndDate}");
                    }

                }
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