using System.ComponentModel.DataAnnotations;
namespace qualityservice.Model.ToolApi
{
    public class Tool
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public string serialNumber { get; set; }
        public string code { get; set; }
        [Required]
        public double lifeCycle { get; set; }
        [Required]
        public double currentLife { get; set; }
        [Required]
        public string unitOfMeasurement { get; set; }
        [Required]
        public int? typeId { get; set; }
        public string typeName { get; set; }
        [Required]
        public string status { get; set; }
    }
}