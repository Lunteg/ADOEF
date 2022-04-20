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
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-87BA9F3\SQLEXPRESS; Database = MusicEF; Trusted_Connection = True;");
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
        public List<Albums> albums;
        public List<Genres> genres;
        public List<Groups> groups;

        public Database()
        {
            genres = new List<Genres>
        {
            new Genres { Id = 1, name = "rock"},
            new Genres { Id = 2, name = "rap"},
            new Genres { Id = 3, name = "metalic"},
            new Genres { Id = 4, name = "hip hop"},
            new Genres { Id = 5, name = "blues"},
            new Genres { Id = 6, name = "pop"},
            new Genres { Id = 7, name = "country"},
            new Genres { Id = 8, name = "funk"},
            new Genres { Id = 9, name = "soul"},
            new Genres { Id = 10, name = "jazz"}
        };

            groups = new List<Groups>
        {
            new Groups { Id = 1, name = "Pink Floyd"},
            new Groups { Id = 2, name = "Led Zeppelin"},
            new Groups { Id = 3, name = "Rolling Stones"},
            new Groups { Id = 4, name = "U2"},
            new Groups { Id = 5, name = "Queen"},
            new Groups { Id = 6, name = "Dire Straits"},
            new Groups { Id = 7, name = "Bruce Springsteen"},
            new Groups { Id = 8, name = "The Beatles"},
            new Groups { Id = 9, name = "Bob Marley"},
            new Groups { Id = 10, name = "AC/DC"}
        };

            albums = new List<Albums>
        {
            new Albums { Id = 1, genre = genres[2], group = groups[3], name = "Sgt. Pepper's Lonely Hearts Club Band", num_of_copies = 10000, year = 1960},
            new Albums { Id = 2, genre = genres[5], group = groups[5], name = "Pet Sounds", num_of_copies = 10000, year = 1960},
            new Albums { Id = 3, genre = genres[8], group = groups[7], name = "Revolver", num_of_copies = 56000, year = 1980},
            new Albums { Id = 4, genre = genres[9], group = groups[8], name = "Highway 61 Revisited", num_of_copies = 200000, year = 1981},
            new Albums { Id = 5, genre = genres[6], group = groups[9], name = "Rubber Soul", num_of_copies = 5400, year = 1981},
            new Albums { Id = 6, genre = genres[5], group = groups[6], name = "What’s Going On", num_of_copies = 654700, year = 1999},
            new Albums { Id = 7, genre = genres[4], group = groups[3], name = "Exile on Main St.", num_of_copies = 64100, year = 2000},
            new Albums { Id = 8, genre = genres[1], group = groups[0], name = "London Calling", num_of_copies = 6477400, year = 2002},
            new Albums { Id = 9, genre = genres[2], group = groups[1], name = "Blonde on Blonde", num_of_copies = 500000, year = 2000},
            new Albums { Id = 10, genre = genres[5], group = groups[0], name = "The Beatles", num_of_copies = 654500, year = 1930},

        };
        }
    }

}


