using System;
using System.Collections.Generic;

namespace CP.API.Dto
{
    public class OrderReturnDTO
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        // public string PaymentType { get; set; }
        // public string ShipperName { get; set; }

        public ICollection<OrderDetailReturnDTO> OrderDetails{ get; set; }
    }
}