using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP.API.Dto
{
    public class OrderDetailReturnDTO
    {
        public int OrderDetailId { get; set; }

      
        public int OrderNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Size { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime BillDate { get; set; }

        public string ProductName { get; set; }

        public int OrderId { get; set; }
    }
}