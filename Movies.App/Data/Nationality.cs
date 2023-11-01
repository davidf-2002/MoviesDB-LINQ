using System.ComponentModel.DataAnnotations;

namespace Movies.App.Data
{
    public class Nationality
    {
        public Nationality()
        {
        }

        // A constructor that takes in initial values for the Nationality entity
        public Nationality(string title) : this()
        {
            Title = title;
        }

        // Properties for the nationality class
        public int NationalityId { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
