using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        public long Duration { get; set; }
        
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
                IdDirector = int.Parse(this.Director),
                IdScenarist = int.Parse(this.Writer),
                Duration = this.Duration,
                IdAgeRating = int.Parse(this.AgeRating),
                IdGenre = int.Parse(this.Genre),
                Title = this.Title,
                IdFirstActor = int.Parse(this.MainActor)
            };
        }
        public static BluRayViewModel ToModel(BluRayDto dto)
        {
            return new BluRayViewModel
            {
                Id = dto.Id,
                Director = dto.IdDirector.ToString(),
                Writer = dto.IdScenarist.ToString(),
                Duration = dto.Duration,
                Support = "Blu-Ray",
                AgeRating = dto.IdAgeRating.ToString(),
                Genre = dto.IdGenre.ToString(),
                Title = dto.Title,
                MainActor = dto.IdFirstActor.ToString()
            };
        }
        
    }
}