using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class HostViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("nom")]
        [Required]
        public string Name { get; set; }
        
        [JsonPropertyName("url")]
        [Required]
        public string Url { get; set; }
    }
}