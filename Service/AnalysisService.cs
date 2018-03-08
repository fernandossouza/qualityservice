using qualityservice.Service.Interface;
using qualityservice.Model;
using qualityservice.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace qualityservice.Service
{
    public class AnalysisService :IAnalysisService
    {
        private readonly IProductionOrderQualityService _productionOrderQualityService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private HttpClient client;

        public AnalysisService(IProductionOrderQualityService productionOrderQualityService,
         ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _productionOrderQualityService = productionOrderQualityService;
            _configuration = configuration;
            client = new HttpClient();
        }
        
        public async Task<Analysis> AddAnalysis(int productionOrderQualityId,Analysis analysis)
        {
            analysis.datetime = DateTime.Now.Ticks;

            var productionQuality = await _productionOrderQualityService.GetProductionOrderQualityId(productionOrderQualityId);

            if(productionQuality.Analysis.Count <=0)
                productionQuality.Analysis = new List<Analysis>();

            analysis.number = productionQuality.Analysis.Count + 1;

            // Espaço para os cálculos
            analysis.status = "reproved";
            analysis.message = "Ajuste de elemento";
            // Fim

            var returnApi = await PutStatusAnalysisForProductionOrder(productionQuality.productionOrderId,analysis.status);
            
            if(returnApi)
            {
                productionQuality.status = analysis.status;

                productionQuality.Analysis.Add(analysis);

                await _context.SaveChangesAsync();

                return analysis;
            }       
            Console.WriteLine("Não foi possivel alterar o status da ordem de produção");
            return null;    

        }

        private async Task<bool> PutStatusAnalysisForProductionOrder(int productionOrderId,string status)
        {
            var builder = new UriBuilder(_configuration["ProductionOrderStatusApi"] + "?productionOrderId="
             + productionOrderId +"&state="+status);
            string url = builder.ToString();
            var result = await client.PutAsync(url,null);
            
            if(result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.NoContent)
                return true;

            return false;
        }
    }
}