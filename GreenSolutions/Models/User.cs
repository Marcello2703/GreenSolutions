using GreenSolutions.Enums;

namespace GreenSolutions.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserType Type {  get; set; }

        public string Name { get; set; }

        public string Email  { get; set; }

        public UserType State{ get; set; }
    }
}
