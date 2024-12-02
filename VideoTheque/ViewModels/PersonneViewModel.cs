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

        public PersonneDto ToDto()
        {
            return new PersonneDto
            {
                Id = this.Id,
                FirstName = this.Firstname,
                LastName = this.Lastname,
                BirthDay = DateTime.Parse(this.Birthdate),
                Nationality = this.Nationality
            };
        }

        public static PersonneViewModel ToModel(PersonneDto dto)
        {
            return new PersonneViewModel
            {
                Id = dto.Id,
                Firstname = dto.FirstName,
                Lastname = dto.LastName,
                Birthdate = dto.BirthDay.ToString("yyyy-MM-dd"),
            };
        }

    }
}