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
        public string LastName { get; set; }
        
        [JsonPropertyName("firstname")]
        [Required]
        public string FirstName { get; set; }
        
        [JsonPropertyName("birthday")]
        [Required]
        public string BirthDay { get; set; }
        
        [JsonPropertyName("nationality")]
        [Required]
        public string Nationality { get; set; }

        public PersonneDto ToDto()
        {
            return new PersonneDto
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                BirthDay = DateTime.Parse(this.BirthDay),
                Nationality = this.Nationality
            };
        }

        public static PersonneViewModel ToModel(PersonneDto dto)
        {
            return new PersonneViewModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDay = dto.BirthDay.ToString("yyyy-MM-dd"),
                Nationality = dto.Nationality
            };
        }

    }
}