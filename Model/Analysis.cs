using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace qualityservice.Model {
    public class Analysis {
        public int analysisId { get; set; }
        public int number { get; set; }
        public long datetime { get; set; }
        public string status { get; set; }
        public string username { get; set; }
        public List<MessageCalculates> messages { get; set; }
        public string cobreFosforoso { get; set; }

        [Required]
        public List<AnalysisComp> comp { get; set; }
    }
}