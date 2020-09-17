namespace CP.API.Helpers
{
    public class SupplierParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
          
        public string FacilityOwnerName { get; set; }
        public string IdNumber { get; set; }
        public string Phone { get; set; }

        


    }
}