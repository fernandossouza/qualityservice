using System.Collections.Generic;
using System.Threading.Tasks;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;

namespace qualityservice.Service.Interface
{
    public interface ICalculateAnalysisService
    {
         Task<List<string>> Calculates(int productionOrderId, double furnaceQuantity,Analysis analysis);
    }
}