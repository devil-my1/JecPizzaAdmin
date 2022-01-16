using System;

namespace JecPizza.Models
{
    public class Member
    {
        public string MemberId { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public bool Sex { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime Dob { get; set; }
    }
}