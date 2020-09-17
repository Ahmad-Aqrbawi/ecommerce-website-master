using System;
using System.ComponentModel.DataAnnotations;

namespace CP.API.Model
{
    public class PhotoForSupplier
    {
        [Key]
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }





    }
}