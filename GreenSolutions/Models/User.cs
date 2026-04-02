using GreenSolutions.Enums;
using System.ComponentModel.DataAnnotations;

namespace GreenSolutions.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public UserType Type {  get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email  { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public UserType State{ get; set; }

        //posteriormente adicionar mais campos atrelados as regras de negócio
    }
}
