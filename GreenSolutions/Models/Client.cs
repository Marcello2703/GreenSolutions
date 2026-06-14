using GreenSolutions.Enums;
using System.ComponentModel.DataAnnotations;

namespace GreenSolutions.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CNPJ { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }

        //public string email { get; set; }

        //refatorar os objetos? client herdando de user e adicionar campo company em client?
    }
}
