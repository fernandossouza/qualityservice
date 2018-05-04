using System.ComponentModel.DataAnnotations;
namespace qualityservice.Model.ProductionOrderApi
{
    public class Product
    {
        [Key]
        public int internalId { get; set; }
        public int productId { get; set; }
        [Required]
        [MaxLength(50)]
        public string productName { get; set; }
        [MaxLength(100)]
        public string productDescription { get; set; }
        [MaxLength(50)]
        public string productCode { get; set; }
        [MaxLength(50)]
        public string productGTIN { get; set; }
    }
}