using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.App.Data
{
    public class Person
    {
        // Properties for the person entity
        public int PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        public int NationalityId { get; set; }

        // Nationality object which defines Person-Nationality relationship (Many-One)
        public Nationality Nationality { get; set; }
    }
}
