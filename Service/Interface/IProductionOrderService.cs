using System.Net;
using System.Threading.Tasks;
using qualityservice.Model.ProductionOrderApi;

namespace qualityservice.Service.Interface
{
    public interface IProductionOrderService
    {
         Task<(ProductionOrder, HttpStatusCode)> getProductionOrderId(int id);
    }
}