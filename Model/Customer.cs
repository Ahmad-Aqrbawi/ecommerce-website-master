using System.Collections.Generic;

namespace CP.API.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string  FirstName{ get; set; }
        public string  LirstName{ get; set; }
        public string  Email{ get; set; }
        public string  Phone{ get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Order> Orders {get; set;}
     
        

    }
}