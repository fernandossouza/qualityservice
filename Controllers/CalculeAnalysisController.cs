using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qualityservice.Service.Interface;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Model;

namespace qualityservice.Controllers
{
    [Route("api/[controller]")]
    public class CalculeAnalysisController : Controller
    {
        private readonly ICalculateAnalysisService _calculateAnalysisService;

        public CalculeAnalysisController(ICalculateAnalysisService calculateAnalysisService)
        {
            _calculateAnalysisService = calculateAnalysisService;
        }

         [HttpPut()]
        public async Task<IActionResult> Put([FromQuery]int productionOrderId,[FromQuery]int furnaceQuantity,[FromBody] Analysis analysis)
        {
            try{
                if (ModelState.IsValid)
                {
                    var returnCalculate = await _calculateAnalysisService.Calculates(productionOrderId,furnaceQuantity,analysis,false);

                     return Ok(returnCalculate);
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