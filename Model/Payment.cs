using System.Collections.Generic;

namespace CP.API.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }

        public ICollection<Order> Orders {get; set;}

         public Product Product { get; set; }
         public int ProductId { get; set; }
    }
}