using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace qualityservice.Model.ProductionOrderApi
{
    public class Phase
    {
        [Key]
        public int internalId { get; set; }
        public int phaseId{get;set;}
        [Required]
        [MaxLength(50)]
        public string phaseName { get; set; }
        [MaxLength(100)]
        public string phaseCode { get; set; }
        public ICollection<PhaseProduct> phaseProducts { get; set; }
    }
}