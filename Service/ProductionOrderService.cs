using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;

namespace qualityservice.Service
{
    public class ProductionOrderService : IProductionOrderService
    {
        private IConfiguration _configuration;
        private HttpClient client = new HttpClient();
        public ProductionOrderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(ProductionOrder, HttpStatusCode)> getProductionOrderId(int id)
        {
            ProductionOrder returnProductionOrder = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["ProductionOrderApi"]);
            string url = builder.ToString() + id;

            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var returnAPI = await client.GetStringAsync(url);
                    returnProductionOrder = JsonConvert.DeserializeObject<ProductionOrder>(returnAPI);
                    return (returnProductionOrder, HttpStatusCode.OK);
                }
                case HttpStatusCode.NotFound:
                    return (returnProductionOrder, HttpStatusCode.NotFound);
                case HttpStatusCode.InternalServerError:
                    return (returnProductionOrder, HttpStatusCode.InternalServerError);
            }
            return (returnProductionOrder, HttpStatusCode.NotFound);
        }
    }
}