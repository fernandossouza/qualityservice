using System.Collections.Generic;
using System.Threading.Tasks;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
namespace qualityservice.Service.Interface
{
    public interface IProductionOrderQualityService
    {
         Task<ProductionOrderQuality> AddProductionOrderQuality(ProductionOrder productionOrder);
         Task<ProductionOrderQuality> updateProductionOrderQuality(int productionOrderQualityId,ProductionOrderQuality productioQualityUpdate);
         Task<(List<ProductionOrderQuality>,int)> GetProductionOrderQualityPerStatus(string status,int startat, int quantity);
         Task<ProductionOrderQuality> GetProductionOrderQualityId(int productionOrderQualityId);
         Task<ProductionOrderQuality> GetProductionOrder(int productionOrderId);
         Task<List<ProductionOrderQuality>> GetProductionOrderQaulityPerRecipeCode(string recipeCode,long startDate,long endDate);
         Task<ProductionOrderQuality> GetProductionOrderQualityNumber(string productionOrderNumber);
         Task<ProductionOrderQuality> setProductionOrderQualityWaiting(int productionOrderId);         
    }
}