using System;

namespace CP.API.Model
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string  DiscountName { get; set; }
        public string CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}