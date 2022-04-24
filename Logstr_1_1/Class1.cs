using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LinqNP
{
    public class MusicContext : DbContext
    {
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Groups> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-87BA9F3\SQLEXPRESS; Database = EfMusic; Trusted_Connection = True;");
            base.OnConfiguring(optionsBuilder);
        }

        public void CreateDfIfNotExist()
        {
            this.Database.EnsureCreated();
        }
    }
    public class Albums
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string name { get; set; }
        public int year { get; set; }
        public int num_of_copies { get; set; }
        public Groups group { get; set; }
        public Genres genre { get; set; }
    }

    public class Genres
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string name { get; set; }
    }

    public class Groups
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string name { get; set; }
    }

    public class Database
    {
        public List<Albums> albums { get; set; }
        public List<Genres> genres { get; set; }
        public List<Groups> groups { get; set; }

        public Database()
        {
            genres = new List<Genres>
        {
            new Genres { name = "rock"},
            new Genres { name = "rap"},
            new Genres { name = "metalic"},
            new Genres { name = "hip hop"},
            new Genres { name = "blues"},
            new Genres { name = "pop"},
            new Genres { name = "country"},
            new Genres { name = "funk"},
            new Genres { name = "soul"},
            new Genres { name = "jazz"}
        };

            groups = new List<Groups>
        {
            new Groups { name = "Pink Floyd"},
            new Groups { name = "Led Zeppelin"},
            new Groups { name = "Rolling Stones"},
            new Groups {  name = "U2"},
            new Groups {  name = "Queen"},
            new Groups { name = "Dire Straits"},
            new Groups { name = "Bruce Springsteen"},
            new Groups { name = "The Beatles"},
            new Groups { name = "Bob Marley"},
            new Groups { name = "AC/DC"}
        };

            albums = new List<Albums>
        {
            new Albums { genre = genres[2], group = groups[3], name = "Sgt. Pepper's Lonely Hearts Club Band", num_of_copies = 10000, year = 1960},
            new Albums { genre = genres[5], group = groups[5], name = "Pet Sounds", num_of_copies = 10000, year = 1960},
            new Albums { genre = genres[8], group = groups[7], name = "Revolver", num_of_copies = 56000, year = 1980},
            new Albums { genre = genres[9], group = groups[8], name = "Highway 61 Revisited", num_of_copies = 200000, year = 1981},
            new Albums { genre = genres[6], group = groups[9], name = "Rubber Soul", num_of_copies = 5400, year = 1981},
            new Albums { genre = genres[5], group = groups[6], name = "What’s Going On", num_of_copies = 654700, year = 1999},
            new Albums { genre = genres[4], group = groups[3], name = "Exile on Main St.", num_of_copies = 64100, year = 2000},
            new Albums { genre = genres[1], group = groups[0], name = "London Calling", num_of_copies = 6477400, year = 2002},
            new Albums { genre = genres[2], group = groups[1], name = "Blonde on Blonde", num_of_copies = 500000, year = 2000},
            new Albums { genre = genres[5], group = groups[0], name = "The Beatles", num_of_copies = 654500, year = 1930},

        };
        }
    }

}


