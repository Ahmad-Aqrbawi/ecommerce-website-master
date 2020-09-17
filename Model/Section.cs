using System.Collections.Generic;

namespace CP.API.Model
{
    public class Section
    {
        public int SectionId  { get; set; }

        public string SectionName { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public ICollection<Product> Products { get; set; }


       
    }
}