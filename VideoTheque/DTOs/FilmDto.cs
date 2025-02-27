namespace VideoTheque.DTOs
{
    public class FilmDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long Duration { get; set; }
        public string MainActor { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string AgeRating { get; set; }
        public string Genre { get; set; }
        public string Support { get; set; }
    }
}