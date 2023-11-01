using Microsoft.EntityFrameworkCore;
using Movies.App.Data;
using System;
using System.Linq;

namespace Movies.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Movies App!");

			// Reset the database each run
			var context = new MoviesContext();
			context.Database.EnsureDeleted();
			context.Database.Migrate();

			var db = new MoviesContext(); // Instance of the EF DbContext class


            // List all nationalities 
            IQueryable<Nationality> allNationalities = db.Nationalities; // Define a LINQ query
			foreach (var n in allNationalities)
			{	// Query performed and entities instantiated
				Console.WriteLine("{0}: {1}", n.NationalityId, n.Title);
			}


            // Find and load British nationality record
            Nationality british = db.Nationalities
				.Where(n => n.Title == "British")
				.FirstOrDefault();

            // Find and load all people with British nationality ordered by last name
            IQueryable<Person> britishPeople = db.People
				.Where(p => p.NationalityId == british.NationalityId)
				.OrderBy(p => p.LastName);

			Movie firstMovie = db.Movies
				//.Include(m => m.Director)		// This line is required to load in the related director entry
				//.Include(m => m.Actors)		// This line is required to load in the related actors
				.FirstOrDefault();
			if (firstMovie != null)
			{
				Person director = firstMovie.Director;  // Without the include this will always be Null 
			}


			// Add a new person
			Person aPerson = db.People
				.Where(p => p.PersonId == 1)
				.FirstOrDefault();
			aPerson.FirstName = "Bob";
			aPerson.LastName = "Roberts";
			aPerson.Birthday = new DateTime(1990, 4, 1);
			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				// failed to make any changes
			}


			// Load the first record that has has the name JJ
			Person jj = db.People
				.Where(p => p.FirstName == "JJ")
				.FirstOrDefault();

            // Find the record for Simon Pegg and update the nationality to Bbritish
            Person simonPegg = db.People
				.Where(p => p.LastName == "Pegg" && p.FirstName == "Simon")
				.FirstOrDefault();
			simonPegg.NationalityId = british.NationalityId; // or simonPegg.Nationality = british;

			Movie starTrek = db.Movies
				.Where(m => m.Title == "Star Trek")
				.FirstOrDefault();
			starTrek.Director = jj; // does not auto-update DirectorId
			db.MovieActors.Add(new MovieActor { Person = simonPegg, Movie = starTrek });
			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				// failed to make any changes
			}

			Nationality kiwi = db.Nationalities
				.Where(n => n.Title == "Kiwi")
				.FirstOrDefault();
			if (kiwi == null)
			{
				kiwi = new Nationality();
				kiwi.Title = "Kiwi"; // Don't set Id; database will decide the primary key
			}

			Person newPerson = new Person();
			newPerson.FirstName = "Karl";
			newPerson.LastName = "Urban";
			newPerson.Birthday = new DateTime(1972, 6, 7);
			newPerson.Nationality = kiwi; // New entity used without being added to database first

			db.People.Add(newPerson); // Adds the Person to the DbContext
			try
			{
				db.SaveChanges(); // Kiwi added to db; newPerson.Id and NationalityId now correct
			}
			catch (DbUpdateException e)
			{
				// failed to make any changes
			}

			db.People.Remove(simonPegg);
			try
			{
				db.SaveChanges(); // Simon gone and Star Trek Actors updated
			}
			catch (DbUpdateException e)
			{
				// failed to make changes
			}
		}
    }
}
