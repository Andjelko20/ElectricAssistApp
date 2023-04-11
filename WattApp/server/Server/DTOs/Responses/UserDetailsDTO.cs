using Server.Models;

namespace Server.DTOs.Responses
{
    public class UserDetailsDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Blocked { get; set; }
        public string Settlement { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Address { get; set; }

        public UserDetailsDTO() { }
        public UserDetailsDTO(UserModel user)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.Username = user.Username;
            this.Role = user.Role.Name;
            this.Blocked = user.Blocked;
            this.Settlement = user.Settlement.Name;
            this.City = user.Settlement.City.Name;
            this.Country = user.Settlement.City.Country.Name;
            this.Address = user.Address;
        }
    }
}
