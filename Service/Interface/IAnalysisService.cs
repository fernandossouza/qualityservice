using System.Threading.Tasks;
using qualityservice.Model;
namespace qualityservice.Service.Interface
{
    public interface IAnalysisService
    {
         Task<Analysis> AddAnalysis(int productionOrderQualityId,Analysis analysis);
    }
}