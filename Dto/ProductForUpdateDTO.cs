namespace CP.API.Dto
{
    public class ProductForUpdateDTO
    {
         public ProductForUpdateDTO(decimal vAT, decimal unitPrice)
        {
            VAT = vAT;
            UnitPrice = unitPrice;
            AmountOfTheAddedTax = (VAT * UnitPrice);
            PriceAfterTax = unitPrice + AmountOfTheAddedTax;
        }
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

       
    }
}