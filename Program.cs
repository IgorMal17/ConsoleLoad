using ConsoleLoad.Models;
using Microsoft.EntityFrameworkCore;
using AppContext = ConsoleLoad.Models.AppContext;

internal class Program {
    private static void Main(string[] args) {

        using (AppContext db = new AppContext())
        {
            // пересоздадим базу данных
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // добавляем начальные данные
            Company microsoft = new Company { Name = "Microsoft" };
            Company google = new Company { Name = "Google" };
            db.Companies.AddRange(microsoft, google);


            User tom = new User { Name = "Tom", Company = microsoft };
            User bob = new User { Name = "Bob", Company = google };
            User alice = new User { Name = "Alice", Company = microsoft };
            User kate = new User { Name = "Kate", Company = google };
            db.Users.AddRange(tom, bob, alice, kate);

            db.SaveChanges();
        }
        //Получим всех работников одной компании
        //Методом Load()
            using (AppContext db = new AppContext())
            {
            Company? company = db.Companies.FirstOrDefault();
            if (company != null)
            {
                db.Users.Where(u => u.CompanyId == company.Id).Load();

                Console.WriteLine($"Company: {company.Name}");
                foreach (var u in company.Users)
                    Console.WriteLine($"User: {u.Name}");
            }
        }

        Console.WriteLine("================================================");

        //Получим всех работников одной компании
        //Collection() и Load()
        using (AppContext db = new AppContext())
        {
            Company? company = db.Companies.FirstOrDefault();
            if (company != null)
            {
                db.Entry(company).Collection(c => c.Users).Load();

                Console.WriteLine($"Company: {company.Name}");
                foreach (var u in company.Users)
                    Console.WriteLine($"User: {u.Name}");
            }
        }

        Console.WriteLine("================================================");

        // получаем первого пользователя и компанию где он работает

        using (AppContext db = new AppContext())
        {
            User? user = db.Users.FirstOrDefault();
            if (user != null)
            {
                db.Entry(user).Reference(u => u.Company).Load();
                Console.WriteLine($"{user.Name} - {user.Company?.Name}");
            }
        }
    }
}