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
        
        [JsonPropertyName("nom")]
        [Required]
        public string LastName { get; set; }
        
        [JsonPropertyName("prenom")]
        [Required]
        public string FirstName { get; set; }
        
        [JsonPropertyName("date-naissance")]
        [Required]
        public string BirthDay { get; set; }
        
        [JsonPropertyName("nationalite")]
        [Required]
        public string Nationality { get; set; }
        
        [JsonPropertyName("nom-prenom")]
        public string FullName => $"{FirstName} {LastName}";


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
                BirthDay = dto.BirthDay.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Nationality = dto.Nationality
            };
        }

    }
}