using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class BluRayViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("realisateur")]
        [Required]
        public string Director { get; set; }
        
        [JsonPropertyName("scenariste")]
        [Required]
        public string Writer { get; set; }
        
        [JsonPropertyName("duree")]
        [Required]
        public int Duration { get; set; }
        
        [JsonPropertyName("support")]
        [Required]
        public string Support { get; set; }
        
        [JsonPropertyName("age-rating")]
        [Required]
        public string AgeRating { get; set; }
        
        [JsonPropertyName("genre")]
        [Required]
        public string Genre { get; set; }
        
        [JsonPropertyName("titre")]
        [Required]
        public string Title { get; set; }
        
        [JsonPropertyName("acteur-principal")]
        [Required]
        public string MainActor { get; set; }
        
        public BluRayDto ToDto()
        {
            return new BluRayDto
            {
                Id = this.Id,
                IdDirector = this.Director,
                IdScenarist = this.Writer,
                Duration = this.Duration,
                IdAgeRating = this.AgeRating,
                IdGenre = this.Genre,
                Title = this.Title,
                IdFirstActor = this.MainActor
            };
        }
        public static BluRayViewModel ToModel(BluRayDto dto)
        {
            return new BluRayViewModel
            {
                Id = dto.Id,
                Director = dto.IdDirector,
                Writer = dto.IdScenarist,
                Duration = dto.Duration,
                Support = "Blu-Ray",
                AgeRating = dto.IdAgeRating,
                Genre = dto.IdGenre,
                Title = dto.Title,
                MainActor = dto.IdFirstActor
            };
        }
        
    }
}