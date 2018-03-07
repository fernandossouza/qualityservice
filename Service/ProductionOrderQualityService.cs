using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using qualityservice.Data;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;

namespace qualityservice.Service
{
    public class ProductionOrderQualityService : IProductionOrderQualityService
    {
        private readonly ApplicationDbContext _context;

        public ProductionOrderQualityService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProductionOrderQuality> AddProductionOrderQuality(ProductionOrder productionOrder)
        {
            ProductionOrderQuality productionOrderQuality = new ProductionOrderQuality();

            

            productionOrderQuality.productionOrderNumber = productionOrder.productionOrderNumber;
            productionOrderQuality.productionOrderId = productionOrder.productionOrderId;
            productionOrderQuality.forno = "Forno1";
            productionOrderQuality.corrida = 1;
            productionOrderQuality.posicao = productionOrder.currentThing.thingName;
            productionOrderQuality.status = "waiting";

            _context.ProductionOrderQualities.Add(productionOrderQuality);
            await _context.SaveChangesAsync();
            return productionOrderQuality;
        }

        // public async Task<ProductionOrderQuality> UpdateProductionOrderQuality(int productionOrderQualityId
        // ,ProductionOrderQuality productionOrderQualityUpdate)
        // {
        //     var productionOrderQualityDb = await _context.ProductionOrderQualities
        //                                         .AsNoTracking()
        //                                         .Include(x=>x.Analysis)
        //                                         .Where(x=>x.productionOrderQualityId == productionOrderQualityId)
        //                                         .FirstOrDefaultAsync();
            
        //     if(productionOrderQualityDb == null || productionOrderQualityDb.productionOrderQualityId != productionOrderQualityId)
        //         return null;

        //     productionOrderQualityDb.Analysis = productionOrderQualityUpdate.Analysis;
        //     productionOrderQualityDb.corrida = pro

        // }
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
    }
}