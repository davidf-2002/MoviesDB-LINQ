using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Movies.App.Data
{
    public class MoviesContext : DbContext
    {
        public string DbPath { get; }

        // Constructor for the MoviesContext class
        public MoviesContext()
        {
            var folder = Environment.SpecialFolder.MyDocuments;     // File location (use MyDocuments to avoid confusion for now)
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "movies.db");
        }

        // DbSet properties for the database entities
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Person> People { get; set; }


        // Method for configuring the database connection (OnConfiguring)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);     // Configure the SQLite connection using the DbPath
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }



        // Method for defining relationships (model configuration)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Movie>()
                .HasOne(m => m.Director)    // Each movie has one director
                .WithMany()                 // Each director can have multiple movies
                .HasForeignKey(m => m.DirectorId)   // Setting foreign key property for the 'Director' in the 'Movie' entity
                .OnDelete(DeleteBehavior.SetNull);  // if director is deleted, the corresponding 'DirectorId' in 'Movie' entity will be set to null

            builder.Entity<MovieActor>()
                .HasKey(a => new { a.PersonId, a.MovieId });    // Defining composite key for the MovieActor entity


            // Handle the many to many relationship between Movie and Person through MovieActors
            builder.Entity<MovieActor>()
                .HasOne(a => a.Movie)       // Each MovieActor is asociated with one movie
                .WithMany(m => m.Actors)    // Each Movie has many Actors
                .HasForeignKey(a => a.MovieId);     // Specifies that 'MovieId' is a foreign key in 'MovieActor' entity
            builder.Entity<MovieActor>()
                .HasOne(a => a.Person)      // Each MovieActor is associated with one Person
                .WithMany()                 // Each Person is associated to many MovieActor
                .HasForeignKey(a => a.PersonId);    // Specifies that 'PersonId' is a foreign key in 'MovieActor' entity


            // Seed data
            builder.Entity<Nationality>().HasData(
                    new Nationality { NationalityId = 1, Title = "British" },
                    new Nationality { NationalityId = 2, Title = "French" },
                    new Nationality { NationalityId = 3, Title = "American" }
                );

            builder.Entity<Person>().HasData(
                new Person { PersonId = 1, NationalityId = 1, Birthday = DateTime.Now, FirstName = "Larry", LastName = "Losser" },
                new Person { PersonId = 2, NationalityId = 1, Birthday = new DateTime(1970, 2, 14), FirstName = "Simon", LastName = "Pegg" },
                new Person { PersonId = 3, NationalityId = 1, Birthday = new DateTime(1976, 7, 19), FirstName = "Benedict", LastName = "Cumberbatch" },
                new Person { PersonId = 4, NationalityId = 2, Birthday = new DateTime(1948, 7, 30), FirstName = "Jean", LastName = "Reno" },
                new Person { PersonId = 5, NationalityId = 3, Birthday = new DateTime(1980, 8, 26), FirstName = "Chris", LastName = "Pine" },
                new Person { PersonId = 6, NationalityId = 3, Birthday = new DateTime(1966, 6, 27), FirstName = "JJ", LastName = "Abrams" }
            );

            builder.Entity<Movie>().HasData(
                new Movie { MovieId = 1, Title = "Star Wars: The Force Awakens", ReleaseDate = new DateTime(2015, 12, 18), DirectorId = 6 },
                new Movie { MovieId = 2, Title = "Star Trek", ReleaseDate = new DateTime(2009, 5, 8), DirectorId = null }
            );
        }
    }
}