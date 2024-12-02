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
        
        public HostDto ToDto()
        {
            return new HostDto
            {
                Id = this.Id,
                Name = this.Name,
                Url = this.Url
            };
        }
        
        public static HostViewModel ToModel(HostDto dto)
        {
            return new HostViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Url = dto.Url
            };
        }
        
    }
}