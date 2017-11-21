namespace P01_BillsPaymentSystem
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data;

    public class StartUp
    {
        public static void Main()
        {
            var db = new BillsPaymentSystemContext();

            using (db)
            {
                db.Database.EnsureDeleted();
            }
        }
    }
}