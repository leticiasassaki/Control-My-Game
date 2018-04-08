using ControlMyGames.Models;
using System;
using System.Linq;

namespace ControlMyGames.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Games.Any())
            {
                return;   // DB has been seeded
            }

            var friends = new Person[]
            {
                new Person{Name = "Maria", Phone= "11111111", Email="maria@hotmail.com"},
                new Person{Name = "Ana", Phone= "11111112", Email="ana@gmail.com"},
                new Person{Name = "Carolina", Phone= "11111113", Email="carolina@yahoo.com"},
                new Person{Name = "Amanda", Phone= "11111114", Email="amanda@gmail.com"}
            };

            foreach (Person p in friends)
                context.Friends.Add(p);

            context.SaveChanges();

            var games = new Game[]
            {
                new Game{Title="Counter-Strike: Global Offensive", Description = "Steam Game"},
                new Game{Title="Playerunknown's Battlegrounds", Description = "Steam Game"},
                new Game{Title="Mario", Description = "Nintendo Game"},
                new Game{Title="Tetris", Description = "Nintendo Game"},
                new Game{Title="Overwatch", Description = "Blizzard Game"},
                new Game{Title="Crash Bandicoot", Description = "Sony Game"},
                new Game{Title="Street Fighter", Description = "Capcom Game"}
            };

            foreach (Game g in games)
                context.Games.Add(g);

            context.SaveChanges();

            var rents = new Rent[]
            {
                new Rent{GameID = games.Single(s => s.Title == "Counter-Strike: Global Offensive").ID,
                         PersonID = friends.Single(s => s.Name == "Maria").ID,
                         RentDate =DateTime.Parse("2018-04-07"),Returned=false},
                new Rent{GameID = games.Single(s => s.Title == "Mario").ID,
                         PersonID = friends.Single(s => s.Name == "Ana").ID,
                         RentDate =DateTime.Parse("2018-04-01"),Returned=false},
                new Rent{GameID = games.Single(s => s.Title == "Tetris").ID,
                         PersonID = friends.Single(s => s.Name == "Carolina").ID,
                         RentDate =DateTime.Parse("2018-04-02"),Returned=false},
                new Rent{GameID = games.Single(s => s.Title == "Street Fighter").ID,
                         PersonID = friends.Single(s => s.Name == "Amanda").ID,
                         RentDate =DateTime.Parse("2018-04-02"), ReturnedDate=DateTime.Parse("2018-04-02"),
                         Returned =true}
            };

            foreach (Rent r in rents)
                context.Rents.Add(r);

            context.SaveChanges();
        }
    }
}
