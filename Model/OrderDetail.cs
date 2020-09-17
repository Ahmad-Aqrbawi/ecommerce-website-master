using System;

namespace CP.API.Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Size { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime BillDate { get; set; }

        public Order Order { get; set; }
        public int OrderId { get; set; }
        
        public Product Product { get; set; }
        public int ProductId { get; set; }

       


    }
}