using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;
using securityfilter;

namespace qualityservice.Controllers {
    [Route ("api/[controller]")]
    public class CalculeAnalysisController : Controller {
        private readonly ICalculateAnalysisService _calculateAnalysisService;

        public CalculeAnalysisController (ICalculateAnalysisService calculateAnalysisService) {
            _calculateAnalysisService = calculateAnalysisService;
        }

        [HttpPut ()]
        [SecurityFilter ("quality__allow_update")]
        public async Task<IActionResult> Put ([FromQuery] int productionOrderId, [FromQuery] int furnaceQuantity, [FromBody] Analysis analysis) {
            try {
                if (ModelState.IsValid) {
                    var returnCalculate = await _calculateAnalysisService.Calculates (productionOrderId, furnaceQuantity, analysis, false);

                    return Ok (returnCalculate);
                }
                return BadRequest (ModelState);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpPut ("recalculate/")]
        public async Task<IActionResult> PutRecalculate ([FromQuery] int productionOrderId, [FromQuery] int productId, [FromQuery] double quantityInput) {
            try {
                if (ModelState.IsValid) {
                    var returnCalculate = await _calculateAnalysisService.Recalculates (productionOrderId, productId, quantityInput);

                    return Ok (returnCalculate);
                }
                return BadRequest (ModelState);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

    }
}