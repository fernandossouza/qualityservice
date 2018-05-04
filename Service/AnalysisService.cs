using qualityservice.Service.Interface;
using qualityservice.Model;
using qualityservice.Data;
using qualityservice.Model.ProductionOrderApi;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Linq;
using Newtonsoft.Json;

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
            var(returnSpecification,stringErro) = await RecipeSpecification(analysis,productionQuality.productionOrderId);

            if(returnSpecification)
                analysis.status = "approved";
            else
                analysis.status = "reproved";

            analysis.message = stringErro;
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

        private async Task<(bool,string)> RecipeSpecification(Analysis analysis, int productionOrderId)
        {
            bool Approved = false;
            var productionOrder = await GetProductionOrder(productionOrderId);

            if(productionOrder == null)
                return (false,"ERRO - Não encontrado a ordem de produção na API");

            foreach(var phase in productionOrder.recipe.phases)
            {
                foreach(var comp in analysis.comp)
                {
                    var recipeComp = phase.phaseProducts.Where(x=>x.product.productId == comp.productId).FirstOrDefault();

                    if(recipeComp == null)
                        return(false,"ERRO - Componente quimico não existe na receita, productId: " + comp.productId );

                    if(recipeComp.minValue >= comp.value || recipeComp.maxValue <= comp.value)
                    {

                    }

                    Approved = true;
                }
            }

            if(Approved)
                return (true,string.Empty);
            else
                return (false,"ERRO - a ordem de produçaõ não possui phase ou não foi possivel obter as analises quimicas");

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

        private async Task<ProductionOrder> GetProductionOrder(int productionOrderId)
        {
            var builder = new UriBuilder(_configuration["ProductionOrderApi"] + productionOrderId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            
            if(result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.NoContent)
                return JsonConvert.DeserializeObject<ProductionOrder>(await client.GetStringAsync(url));

            return null;
        }
    }
}