using System;

namespace CP.API.Dto
{
    public class OrderRegisterDTO
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }

        public int PaymentId { get; set; }
        public int ShipperId { get; set; }

        public OrderRegisterDTO()
        {
        
            OrderDate = DateTime.Now;
            
        }
    }
}