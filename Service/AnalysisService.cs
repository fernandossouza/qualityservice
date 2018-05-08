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
        private readonly ICalculateAnalysisService _calculateAnalysisService;
        private HttpClient client;

        public AnalysisService(IProductionOrderQualityService productionOrderQualityService,
         ApplicationDbContext context, IConfiguration configuration,ICalculateAnalysisService calculateAnalysisService)
        {
            _context = context;
            _productionOrderQualityService = productionOrderQualityService;
            _configuration = configuration;
            _calculateAnalysisService = calculateAnalysisService;
            client = new HttpClient();
        }

        public async Task<Analysis> AddAnalysis(int productionOrderId,Analysis analysis)
        {
            analysis.datetime = DateTime.Now.Ticks;

            var productionQuality = await _productionOrderQualityService.GetProductionOrder(productionOrderId);

            if(productionQuality.Analysis.Count <=0)
                productionQuality.Analysis = new List<Analysis>();

            analysis.number = productionQuality.Analysis.Count + 1;

            // Espaço para os cálculos
            var(returnSpecification,stringRetorno) = await RecipeSpecification(analysis,productionQuality.productionOrderId
            ,productionQuality.qntForno);

            if(returnSpecification)
                analysis.status = "approved";
            else
                analysis.status = "reproved";
            foreach (var item in stringRetorno)
            {
                MessageCalculates message = new MessageCalculates();
                message.message = item.ToString();
                
                if(analysis.messages == null)
                    analysis.messages = new List<MessageCalculates>();

                analysis.messages.Add(message);
            }
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

        private async Task<(bool,List<string>)> RecipeSpecification(Analysis analysis, int productionOrderId, double qtdForno)
        {
            bool Approved = false;
            List<string> stringRetorno = new List<string>(); 
            var productionOrder = await GetProductionOrder(productionOrderId);

            if(productionOrder == null)
            {
                stringRetorno.Add("ERRO - Não encontrado a ordem de produção na API");
                return (false,stringRetorno);
            }
            foreach(var phase in productionOrder.recipe.phases)
            {
                foreach(var comp in analysis.comp)
                {
                    var recipeComp = phase.phaseProducts.Where(x=>x.product.productId == comp.productId).FirstOrDefault();

                    if(recipeComp == null)
                    {
                        stringRetorno.Add("ERRO - Componente quimico não existe na receita, productId: " + comp.productId);
                        return(false,stringRetorno);
                    }
                    comp.productName = recipeComp.product.productName;
                    if(recipeComp.minValue >= comp.value || recipeComp.maxValue <= comp.value)
                    {
                        Approved = false;
                        break;
                    }

                    Approved = true;
                }
            }

            if(Approved)
                return (Approved,stringRetorno);

            stringRetorno = await _calculateAnalysisService.Calculates(productionOrder.productionOrderId,qtdForno,analysis);           
            

            return (Approved,stringRetorno);

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