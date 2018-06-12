using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qualityservice.Model;
using qualityservice.Service.Interface;
using securityfilter;

namespace qualityservice.Controllers {
    [Route ("api/ProductionOrderQuality/[controller]")]
    public class AnalysisController : Controller {
        private readonly IAnalysisService _analysisService;

        public AnalysisController (IAnalysisService analysisService) {
            _analysisService = analysisService;
        }

        [HttpPost ("productionOrder/{productionOrderId}")]
        [SecurityFilter ("quality__allow_update")]
        public async Task<IActionResult> PostProductionOrder (int productionOrderId, [FromBody] Analysis analysis) {
            try {
                if (ModelState.IsValid) {
                    Console.WriteLine("analysis: ");
                    Console.WriteLine(analysis.ToString());
                    var analysisDb = await _analysisService.AddAnalysis (productionOrderId, analysis);

                    return Created ($"api/productionOrderQuality/{analysisDb.analysisId}", analysisDb);
                }
                return BadRequest (ModelState);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

    }
}