using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using qualityservice.Data;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Model.ToolApi;
using qualityservice.Service.Interface;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace qualityservice.Service
{
    public class ProductionOrderQualityService : IProductionOrderQualityService
    {
        private readonly ApplicationDbContext _context;
        private HttpClient client;
        private readonly IConfiguration _configuration;

        public ProductionOrderQualityService(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            client = new HttpClient();
        }
        public async Task<ProductionOrderQuality> AddProductionOrderQuality(ProductionOrder productionOrder)
        {
            ProductionOrderQuality productionOrderQuality = new ProductionOrderQuality();

            var tool = await GetToolApi(productionOrder.currentThing.thingId);

            if(tool == null)
            {
                return null;
            }


            productionOrderQuality.productionOrderNumber = productionOrder.productionOrderNumber;
            productionOrderQuality.productionOrderId = productionOrder.productionOrderId;
            productionOrderQuality.forno = tool.FirstOrDefault().name;
            productionOrderQuality.corrida = Convert.ToInt32(tool.FirstOrDefault().currentLife);
            productionOrderQuality.posicao = productionOrder.currentThing.thingName;
            productionOrderQuality.status = "waiting";

            _context.ProductionOrderQualities.Add(productionOrderQuality);
            await _context.SaveChangesAsync();
            return productionOrderQuality;
        }      

        public async Task<ProductionOrderQuality> updateProductionOrderQuality(int productionOrderQualityId,
         ProductionOrderQuality productioQualityUpdate)
        {
            var productionOrderQualityDB = await _context.ProductionOrderQualities
                                                .Where(x=>x.productionOrderQualityId == productionOrderQualityId)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync();

            if (productionOrderQualityDB == null && 
            productionOrderQualityDB.productionOrderQualityId != productionOrderQualityId)
            {
                return null;
            }

            productionOrderQualityDB.forno = productioQualityUpdate.forno;
            productionOrderQualityDB.corrida = productioQualityUpdate.corrida;
            productionOrderQualityDB.posicao = productioQualityUpdate.posicao;
            productionOrderQualityDB.productionOrderNumber = productioQualityUpdate.productionOrderNumber;
            productionOrderQualityDB.status = productioQualityUpdate.status;

            _context.ProductionOrderQualities.Update(productionOrderQualityDB);
            await _context.SaveChangesAsync();
            return productionOrderQualityDB;
        }


        public async Task<(List<ProductionOrderQuality>,int)> GetProductionOrderQualityPerStatus(string status,int startat, int quantity)
        {
            var productionOrderQualityList = await _context.ProductionOrderQualities.Where(x=>x.status == status)
                                                    .Skip(startat).Take(quantity).ToListAsync();
            var total = await _context.ProductionOrderQualities.Where(x=>x.status == status).CountAsync();

            return(productionOrderQualityList,total);

        }
        public async Task<ProductionOrderQuality> GetProductionOrderQualityId(int productionOrderQualityId)
        {
            var productionOrderQuality = await _context.ProductionOrderQualities
                                                .Include(x=>x.Analysis)
                                                .Where(x=>x.productionOrderQualityId == productionOrderQualityId)
                                                .FirstOrDefaultAsync();
            
            return productionOrderQuality;
            
        }
        public async Task<ProductionOrderQuality> GetProductionOrderQualityNumber(string productionOrderNumber)
        {
            var productionOrderQuality = await _context.ProductionOrderQualities
                                                .Include(x=>x.Analysis)
                                                .Where(x=>x.productionOrderNumber == productionOrderNumber)
                                                .FirstOrDefaultAsync();
            
            return productionOrderQuality;

        }

        private async Task<List<Tool>> GetToolApi(int thingId)
        {
            List<Tool> returnTool = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["toolApi"] + "/" + thingId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnTool = JsonConvert.DeserializeObject<List<Tool>>(await client.GetStringAsync(url));
                    return (returnTool);
                case HttpStatusCode.NotFound:
                    return (returnTool);
                case HttpStatusCode.InternalServerError:
                    return (returnTool);
            }

            return returnTool;
        }
    }
}