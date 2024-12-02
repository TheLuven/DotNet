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
        public int IdDirector { get; set; }
        
        [JsonPropertyName("scenariste")]
        [Required]
        public int IdWriter { get; set; }
        
        [JsonPropertyName("duree")]
        [Required]
        public long Duration { get; set; }
        
        [JsonPropertyName("support")]
        [Required]
        public int IdSupport { get; set; }
        
        [JsonPropertyName("age-rating")]
        [Required]
        public int IdAgeRating { get; set; }
        
        [JsonPropertyName("genre")]
        [Required]
        public int IdGenre { get; set; }
        
        [JsonPropertyName("titre")]
        [Required]
        public string Title { get; set; }
        
        [JsonPropertyName("acteur-principal")]
        [Required]
        public int IdMainActor { get; set; }
        
        public BluRayDto ToDto()
        {
            return new BluRayDto
            {
                Id = this.Id,
                IdDirector = this.IdDirector,
                IdScenarist = this.IdWriter,
                Duration = this.Duration,
                IdAgeRating = this.IdAgeRating,
                IdGenre = this.IdGenre,
                Title = this.Title,
                IdFirstActor = this.IdMainActor
            };
        }
        public static BluRayViewModel ToModel(BluRayDto dto)
        {
            return new BluRayViewModel
            {
                Id = dto.Id,
                IdDirector = dto.IdDirector,
                IdWriter = dto.IdScenarist,
                Duration = dto.Duration,
                IdSupport = 1,
                IdAgeRating = dto.IdAgeRating,
                IdGenre = dto.IdGenre,
                Title = dto.Title,
                IdMainActor = dto.IdFirstActor
            };
        }
        
    }
}