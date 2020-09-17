using System;

namespace CP.API.Dto
{
    public class OrderDetailRegisterDTO
    {
         public OrderDetailRegisterDTO()
        {
            BillDate = DateTime.Today;
            decimal dd = (this.Quantity + this.Price);
            Total = dd;
       
        }
        public int OrderDetailId { get; set; }
        public int OrderNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Size { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime BillDate { get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }
       


    }
}