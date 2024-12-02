using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class FilmViewModel
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
        
    }
}