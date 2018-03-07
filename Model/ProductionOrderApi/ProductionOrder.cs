using System.ComponentModel.DataAnnotations;
namespace qualityservice.Model.ProductionOrderApi
{
    public class ProductionOrder
    {
        [Required]
        public int productionOrderId { get; set; }
        //public Recipe recipe { get; set; }
        [Required]
        public string productionOrderNumber { get; set; }
        [Required]
        public int? productionOrderTypeId { get; set; }
        [Required]
        public string typeDescription { get; set; }
        public int? quantity { get; set; }
        [Required]
        public string currentStatus { get; set; }
        [Required]
        public int? currentThingId { get; set; }
        [Required]
        public Thing currentThing { get; set; }
    }
}