using System.Collections.Generic;
using System.Threading.Tasks;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;

namespace qualityservice.Service
{
    public class ProductionOrderQualityService : IProductionOrderQualityService
    {
        public async Task<ProductionOrderQuality> addProductionOrderQuality(ProductionOrder productionOrder)
        {

        }
        public async Task<List<ProductionOrderQuality>> GetProductionOrderQualityPerStatus(string status = null)
        {

        }
        public async Task<ProductionOrderQuality> GetProductionOrderQualityId(int productionOrderQualityId)
        {
            
        }
        public async Task<ProductionOrderQuality> GetProductionOrderQualityNumber(string productionOrderNumber)
        {

        }
    }
}