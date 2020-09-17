using System.Collections.Generic;

namespace CP.API.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public ICollection<Section> Sections { get; set; }


    }
}