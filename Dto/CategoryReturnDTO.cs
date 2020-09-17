using System.Collections.Generic;

namespace CP.API.Dto
{
    public class CategoryReturnDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        
        public ICollection<SectionReturnDTO> Sections { get; set; }
        
    }
}