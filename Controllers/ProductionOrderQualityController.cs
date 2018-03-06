using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qualityservice.Service.Interface;
using qualityservice.Model.ProductionOrderApi;

namespace qualityservice.Controllers
{
    [Route("api/[controller]")]
    public class ProductionOrderQualityController : Controller
    {
        private readonly IProductionOrderQualityService _productionOrderQualityService;

        public ProductionOrderQualityController(IProductionOrderQualityService productionOrderQualityService)
        {
            _productionOrderQualityService = productionOrderQualityService;
        }

        [HttpGet("{status/status}")]
        public async Task<IActionResult> Get(string status,[FromQuery]int startat,[FromQuery]int quantity)
        {
            try{
                if(quantity == 0)
                    quantity =50;    


                var (productionQualityList,total) = await _productionOrderQualityService
                                                            .GetProductionOrderQualityPerStatus(status,
                                                            startat,quantity);

                if(productionQualityList == null)
                    return NotFound();

                return Ok(new {values = productionQualityList, total = total});
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id/id}")]
        public async Task<IActionResult> Get(int id)
        {
            try{
                
                var productionQuality = await _productionOrderQualityService
                                                            .GetProductionOrderQualityId(id);

                if(productionQuality == null)
                    return NotFound();

                return Ok(productionQuality);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{number/number}")]
        public async Task<IActionResult> Get(string number)
        {
            try{
                
                var productionQuality = await _productionOrderQualityService
                                                            .GetProductionOrderQualityNumber(number);

                if(productionQuality == null)
                    return NotFound();

                return Ok(productionQuality);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductionOrder productionOrder)
        {
            try{
                if (ModelState.IsValid)
                {
                    var productionQuality = await _productionOrderQualityService
                                                            .addProductionOrderQuality(productionOrder);

                     return Created($"api/productionOrderQuality/{productionQuality.productionOrderQualityId}", productionQuality);
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