using System.ComponentModel.DataAnnotations;

namespace qualityservice.Model
{
    public class Analysis
    {
        public int analysisId{get;set;}
        public int number{get;set;}
        public long datetime{get;set;}
        public string status{get;set;}
        public string message{get;set;}
        [Required]
        public double elem_Pb{get;set;}
        [Required]
        public double elem_Sn{get;set;}
        [Required]
        public double elem_Ni{get;set;}
        [Required]
        public double elem_Fe{get;set;}
        [Required]
        public double elem_Cu{get;set;}
    }
}