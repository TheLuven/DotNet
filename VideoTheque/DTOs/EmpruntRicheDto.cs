using VideoTheque.DTOs;

namespace VideoTheque.DTOs
{
    public class EmpruntRicheDto
    {
        public string Title { get; set; }
        public long Duration { get; set; }
        public GenreDto Genre { get; set; }
        public PersonneDto Director { get; set; }
        public PersonneDto FirstActor { get; set; }
        public PersonneDto Scenarist { get; set; }
        public AgeRatingDto AgeRating { get; set; }
    }
}