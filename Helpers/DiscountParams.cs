using System;

namespace CP.API.Helpers
{
    public class DiscountParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
          
         public string  DiscountName { get; set; }
        public string CouponCode { get; set; }
     
    }
}