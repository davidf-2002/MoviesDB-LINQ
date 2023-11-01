namespace Movies.App.Data
{
    public class MovieActor
    {
        // Properties for MovieActor entity
        public int PersonId { get; set; }
        public int MovieId { get; set; }


        // Person + Movie objects which defines Many-One relationships between MovieActor + Person/Movie entities
        public Person Person { get; set; }
        public Movie Movie { get; set; }
    }
}
