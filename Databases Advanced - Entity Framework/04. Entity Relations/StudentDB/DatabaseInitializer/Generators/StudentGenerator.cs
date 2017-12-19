namespace DatabaseInitializer.Generators
{
    using P01_StudentSystem.Data;
    using P01_StudentSystem.Data.Models;

    public class StudentGenerator
    {
        public static void InitialStudentSeed(StudentSystemContext db, int count)
        {
            for (int i = 0; i < count; i++)
            {
                db.Students.Add(NewStudent());
                db.SaveChanges();
            }
        }

        public static Student NewStudent()
        {
            var name = NameGenerator.FirstName() + " " + NameGenerator.LastName();
            var registeredOn = DateGenerator.GenerateDate();
            var phoneNumber = PhoneNumberGenerator.NewPhoneNumber();

            Student customer = new Student()
            {
                Name = name,
                RegisteredOn = registeredOn,
                PhoneNumber = phoneNumber
            };

            return customer;
        }
    }
}