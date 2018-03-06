namespace qualityservice.Model.ProductionOrderApi
{
    public class ProductionOrder
    {
        public int productionOrderId { get; set; }
        //public Recipe recipe { get; set; }
        public string productionOrderNumber { get; set; }
        public int? productionOrderTypeId { get; set; }
        public string typeDescription { get; set; }
        public int? quantity { get; set; }
        public string currentStatus { get; set; }
    }
}