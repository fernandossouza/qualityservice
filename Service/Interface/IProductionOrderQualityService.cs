using System.Collections.Generic;
using System.Threading.Tasks;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
namespace qualityservice.Service.Interface
{
    public interface IProductionOrderQualityService
    {
         Task<ProductionOrderQuality> addProductionOrderQuality(ProductionOrder productionOrder);
         Task<List<ProductionOrderQuality>> GetProductionOrderQualityPerStatus(string status = null);
         Task<ProductionOrderQuality> GetProductionOrderQualityId(int productionOrderQualityId);
         Task<ProductionOrderQuality> GetProductionOrderQualityNumber(string productionOrderNumber);         
    }
}