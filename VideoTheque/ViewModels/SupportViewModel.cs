using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class SupportViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        [Required]
        public string Name { get; set; }

        public SupportDto ToDto()
        {
            return new SupportDto
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        public static SupportViewModel ToModel(SupportDto dto)
        {
            return new SupportViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
        
    }
}