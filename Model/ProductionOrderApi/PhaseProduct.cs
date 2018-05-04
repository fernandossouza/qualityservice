using System.ComponentModel.DataAnnotations;
namespace qualityservice.Model.ProductionOrderApi
{
    public class PhaseProduct
    {
        [Key]
        public int internalId { get; set; }
        [Required]
        public double minValue { get; set; }
        [Required]
        public double maxValue { get; set; }
        [Required]
        [MaxLength(50)]
        public string measurementUnit { get; set; }
        [Required]
        public Product product { get; set; }
    }
}