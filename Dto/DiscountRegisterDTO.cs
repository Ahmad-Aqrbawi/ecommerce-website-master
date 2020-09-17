using System;

namespace CP.API.Dto
{
    public class DiscountRegisterDTO
    {
        public string  DiscountName { get; set; }
        public string CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}