using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movies.App.Data
{
    public class Movie
    {
        public Movie()
        {

        }

        // A constructor that takes in initial values for the Movie entity
        public Movie(string title, DateTime releaseDate, int directorId) : this()
        {
            Title = title;
            ReleaseDate = releaseDate;
            DirectorId = directorId;
        }

        // Properties for movie class
        [Required]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int? DirectorId { get; set; }    // nullable
        public Person Director { get; set; }

        // This will list all the actors that play in the specific film
        public List<MovieActor> Actors { get; set; } = new List<MovieActor>();
    }
}
