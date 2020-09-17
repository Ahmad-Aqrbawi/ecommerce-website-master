using System.Collections.Generic;
using CP.API.Model;

namespace CP.API.Dto
{
    public class ProductRegisterDTO
    {
        public int ProductNumber { get; set; }
        public string SKU { get; set; }
        public string ProductNameEnglish { get; set; }
        public string ProductNameArabic { get; set; }
        public string ProductDescriptionEnglish { get; set; }
        public string ProductDescriptionArabic { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VAT { get; set; }
        public decimal AmountOfTheAddedTax { get; set; }
        public decimal PriceAfterTax { get; set; }
        public int Quantity { get; set; }
        public decimal ActualCost { get; set; }
        public int SectionId { get; set; }
        public int SupplierId { get; set; }

        public ProductRegisterDTO(decimal vAT , decimal unitPrice  )
        {
            VAT = vAT;
            UnitPrice = unitPrice;
            AmountOfTheAddedTax =  (VAT * UnitPrice);
            PriceAfterTax = unitPrice + AmountOfTheAddedTax;
        }

    }
}