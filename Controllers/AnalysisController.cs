using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qualityservice.Service.Interface;
using qualityservice.Model;

namespace qualityservice.Controllers
{
    [Route("api/ProductionOrderQuality/[controller]")]
    public class AnalysisController : Controller
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpPost("{productionOrderQualityId}")]
        public async Task<IActionResult> Post(int productionOrderQualityId,[FromBody] Analysis analysis)
        {
            try{
                if (ModelState.IsValid)
                {
                    var analysisDb = await _analysisService.AddAnalysis(productionOrderQualityId,analysis);

                     return Created($"api/productionOrderQuality/{analysisDb.analysisId}", analysisDb);
                }   
            return BadRequest(ModelState);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}