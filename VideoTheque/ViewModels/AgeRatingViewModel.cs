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

        public AgeRatingDto ToDto()
        {
            return new AgeRatingDto
            {
                Id = this.Id,
                Name = this.Name,
                Abreviation = this.Abreviation
            };
        }

        public static AgeRatingViewModel ToModel(AgeRatingDto dto)
        {
            return new AgeRatingViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Abreviation = dto.Abreviation
            };
        }
    }
}
