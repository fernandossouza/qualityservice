namespace qualityservice.Model.ProductionOrderApi
{
    public class Thing
    {
        
        public int thingId { get; set; }
        public string thingName { get; set; }
        public string thingCode { get; set; }
        public int? currentThingId { get; set; }
    }
}