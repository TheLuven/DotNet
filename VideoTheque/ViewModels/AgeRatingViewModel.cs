using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class AgeRatingViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        [Required]
        public string Name { get; set; }

        [JsonPropertyName("abreviation")]
        [Required]
        public string Abreviation { get; set; }
    }
}
