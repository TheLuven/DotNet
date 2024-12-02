using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class PersonneViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("surname")]
        [Required]
        public string Prénom { get; set; }
        
        [JsonPropertyName("firstname")]
        [Required]
        public string Nom { get; set; }
        
        [JsonPropertyName("birthdate")]
        [Required]
        public string DateNaissance { get; set; }
        
        [JsonPropertyName("nationality")]
        [Required]
        public string Nationalité { get; set; }

        public PersonneDto ToDto()
        {
            return new PersonneDto
            {
                Id = this.Id,
                FirstName = this.Prénom,
                LastName = this.Nom,
                BirthDay = DateTime.Parse(this.DateNaissance),
                Nationality = this.Nationalité
            };
        }

    }
}