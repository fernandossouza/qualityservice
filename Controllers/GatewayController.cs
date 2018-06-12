using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using qualityservice.Service.Interface;
using securityfilter;

namespace qualityservice.Controllers {
    [Route ("")]
    public class GatewayController : Controller {
        private IConfiguration _configuration;
        private IProductionOrderService _productionOrderService;
        public GatewayController (IConfiguration configuration, IProductionOrderService productionOrderService) {
            _configuration = configuration;
            _productionOrderService = productionOrderService;
        }

        [SecurityFilter ("quality__allow_read")]
        [HttpGet ("gateway/productionorder/{id}")]
        [Produces ("application/json")]
        public async Task<IActionResult> GetProductionOrder (int id) {
            var (productionOrder, resultCode) = await _productionOrderService.getProductionOrderId (id);
            switch (resultCode) {
                case HttpStatusCode.OK:
                    return Ok (productionOrder);
                case HttpStatusCode.NotFound:
                    return NotFound ();
            }
            return StatusCode (StatusCodes.Status500InternalServerError);
        }
    }
}