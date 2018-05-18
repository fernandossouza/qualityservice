using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;
using securityfilter;

namespace qualityservice.Controllers {
    [Route ("api/[controller]")]
    public class ProductionOrderQualityController : Controller {
        private readonly IProductionOrderQualityService _productionOrderQualityService;

        public ProductionOrderQualityController (IProductionOrderQualityService productionOrderQualityService) {
            _productionOrderQualityService = productionOrderQualityService;
        }

        [HttpGet ("status/{status}")]
        [SecurityFilter ("quality__allow_update")]
        public async Task<IActionResult> Get (string status, [FromQuery] int startat, [FromQuery] int quantity) {
            try {
                if (quantity == 0)
                    quantity = 50;

                var (productionQualityList, total) = await _productionOrderQualityService
                    .GetProductionOrderQualityPerStatus (status,
                        startat, quantity);

                if (productionQualityList == null)
                    return NotFound ();

                return Ok (new { values = productionQualityList, total = total });
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("recipeCode/{recipeCode}")]
        [SecurityFilter ("quality__allow_read")]
        public async Task<IActionResult> GetRecipeCode (string recipeCode, [FromQuery] long startDate, [FromQuery] long endDate) {
            try {

                var productionQualityList = await _productionOrderQualityService
                    .GetProductionOrderQaulityPerRecipeCode (recipeCode,
                        startDate, endDate);

                if (productionQualityList == null)
                    return NotFound ();

                return Ok (productionQualityList);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("Date")]
        [SecurityFilter ("quality__allow_read")]
        public async Task<IActionResult> GetDate ([FromQuery] long startDate, [FromQuery] long endDate) {
            try {

                var productionQualityList = await _productionOrderQualityService
                    .GetProductionOrderQaulityPerDate (startDate, endDate);

                if (productionQualityList == null)
                    return NotFound ();

                return Ok (productionQualityList);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("id/{id}")]
        [SecurityFilter ("quality__allow_read")]
        public async Task<IActionResult> Get (int id) {
            try {

                var productionQuality = await _productionOrderQualityService
                    .GetProductionOrderQualityId (id);

                if (productionQuality == null)
                    return NotFound ();

                return Ok (productionQuality);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("number/{number}")]
        [SecurityFilter ("quality__allow_read")]
        public async Task<IActionResult> Get (string number) {
            try {

                var productionQuality = await _productionOrderQualityService
                    .GetProductionOrderQualityNumber (number);

                if (productionQuality == null)
                    return NotFound ();

                return Ok (productionQuality);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpGet ("productionOrder/{productionOrderId}")]
        [SecurityFilter ("quality__allow_read")]
        public async Task<IActionResult> GetProductionOrderId (int productionOrderId) {
            try {

                var productionQuality = await _productionOrderQualityService
                    .GetProductionOrder (productionOrderId);

                if (productionQuality == null)
                    return NotFound ();

                return Ok (productionQuality);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpPost]
        [SecurityFilter ("quality__allow_update")]
        public async Task<IActionResult> Post ([FromBody] ProductionOrder productionOrder) {
            try {
                if (ModelState.IsValid) {
                    var productionQuality = await _productionOrderQualityService
                        .AddProductionOrderQuality (productionOrder);

                    return Created ($"api/productionOrderQuality/{productionQuality.productionOrderQualityId}", productionQuality);
                }
                return BadRequest (ModelState);
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }

        [HttpPost ("Waiting")]
        [SecurityFilter ("quality__allow_update")]
        public async Task<IActionResult> PostWaiting ([FromBody] ProductionOrder productionOrder) {
            try {

                var productionQuality = await _productionOrderQualityService
                    .setProductionOrderQualityWaiting (productionOrder.productionOrderId);
                if (productionQuality != null)
                    return Ok (productionQuality);
                return StatusCode (500, "");
            } catch (Exception ex) {
                return StatusCode (500, ex.Message);
            }
        }
    }
}