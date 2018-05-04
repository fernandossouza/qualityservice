using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace qualityservice.Service
{
    public class CalculateAnalysisService : ICalculateAnalysisService
    {
        private readonly IConfiguration _configuration;
        private HttpClient client;

        public CalculateAnalysisService (IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
        }
        

        public async Task<List<string>> Calculates(int productionOrderId, int furnaceQuantity,Analysis analysis)
        {
            List<string> stringReturn = new List<string>();
            var productionOrder = await GetProductionOrder(productionOrderId);

            if(productionOrder == null)
            {
                stringReturn.Add("Ordem não encontrada");
            }
            else
            {
           
            stringReturn.AddRange(await CalculatesAnalysis(analysis,productionOrder,furnaceQuantity));
            }
            return stringReturn;
        }
        private async Task<List<string>> CalculatesAnalysis(Analysis analysisReal, ProductionOrder productionOrder, int furnaceQuantity)
        {
            List<string> stringReturn = new List<string>();
            double maxForno = Convert.ToDouble(_configuration["maxForno"]);
            AnalysisComp analysisCobreEstimada = new AnalysisComp();
            double qtdForno = 0;
            

            foreach(var compAnalysis in analysisReal.comp)
            {
                compAnalysis.valueKg = (compAnalysis.value / 100) * furnaceQuantity;
            }

            List<AnalysisComp> analysisRecipeList = new List<AnalysisComp>();
            foreach(var compRecipe in productionOrder.recipe.phases.FirstOrDefault().phaseProducts)
            {
                AnalysisComp analysisRecipe = new AnalysisComp();
                analysisRecipe.productId = compRecipe.product.productId;
                analysisRecipe.productName = compRecipe.product.productName;
                analysisRecipe.value = ((compRecipe.maxValue - compRecipe.minValue) / 2) + compRecipe.minValue;
                analysisRecipe.valueKg = (analysisRecipe.value / 100) * maxForno;

                analysisRecipeList.Add(analysisRecipe);
            }

            List<AnalysisComp> analysisEstimadaList = new List<AnalysisComp>();

            foreach(var comp in analysisRecipeList)
            {
                

                AnalysisComp analysisEstimada = new AnalysisComp();
                analysisEstimada.productId = comp.productId;
                analysisEstimada.productName = comp.productName;

                AnalysisComp compAnalysis = analysisReal.comp.Where(x=>x.productId == comp.productId).FirstOrDefault();   

                if(compAnalysis == null)
                {
                    compAnalysis = new AnalysisComp();
                    compAnalysis.productId = comp.productId;
                    compAnalysis.productName = comp.productName;
                    compAnalysis.value = 0;
                    compAnalysis.valueKg = 0;
                }             

                analysisEstimada.value = (comp.valueKg /maxForno) * 100;

                analysisEstimada.valueKg = comp.valueKg - compAnalysis.valueKg;

                if(analysisEstimada.valueKg < 0)
                    qtdForno = qtdForno + (analysisEstimada.valueKg * -1) + compAnalysis.valueKg;
                else
                    qtdForno = qtdForno + analysisEstimada.valueKg + compAnalysis.valueKg;

                if(qtdForno > maxForno)
                {
                    stringReturn = new List<string>();
                    stringReturn.Add("Excesso de Carga!");
                    return stringReturn;
                }

                if(analysisEstimada.valueKg > 0)
                {
                    string quantidadeAdicionar = string.Empty;
                    quantidadeAdicionar = quantidadeAdicionar + " Adicionar " + analysisEstimada.valueKg;
                    quantidadeAdicionar = quantidadeAdicionar + " Kg de " + analysisEstimada.productName + " ";

                    stringReturn.Add(quantidadeAdicionar);
                }
            }

            return stringReturn;
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