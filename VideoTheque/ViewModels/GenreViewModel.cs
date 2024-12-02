using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class GenreViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        [Required]
        public string Name { get; set; }

        public GenreDto ToDto()
        {
            return new GenreDto
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        public static GenreViewModel ToModel(GenreDto dto)
        {
            return new GenreViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
