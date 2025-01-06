using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class EmpruntViewModel
    {
        [JsonPropertyName("filmId")]
        [Required]
        public int Id { get; set; }
        
        [JsonPropertyName("titre")]
        [Required]
        public string Title { get; set; }
        
        [JsonPropertyName("genre")]
        [Required]
        public string Genre { get; set; }
        
        [JsonPropertyName("realisateur")]
        [Required]
        public string Director { get; set; }
        
        [JsonPropertyName("acteur-principal")]
        [Required]
        public string FirstActor { get; set; }
        
    }
}