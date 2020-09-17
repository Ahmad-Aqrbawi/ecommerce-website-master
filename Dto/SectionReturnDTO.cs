using System.Collections.Generic;

namespace CP.API.Dto
{
    public class SectionReturnDTO
    {
          public int SectionId  { get; set; }
        public string SectionName { get; set; }
        public string CategoryName { get; set; }

         public ICollection<ProductReturnDTO> Products { get; set; }

    }
}