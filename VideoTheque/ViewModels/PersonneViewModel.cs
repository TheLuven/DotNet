using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Mapster;
using Newtonsoft.Json;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class PersonneViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("lastname")]
        [Required]
        public string Lastname { get; set; }
        
        [JsonPropertyName("firstname")]
        [Required]
        public string Firstname { get; set; }
        
        [JsonPropertyName("birthdate")]
        [Required]
        public string Birthdate { get; set; }
        
        [JsonPropertyName("nationality")]
        [Required]
        public string Nationality { get; set; }

        public PersonneDto ToDto(PersonneDto dto)
        {
            return new PersonneDto
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDay = dto.BirthDay,
                Nationality = dto.Nationality
            };
        }

    }
}