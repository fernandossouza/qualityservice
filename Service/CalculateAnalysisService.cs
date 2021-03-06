using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using qualityservice.Model;
using qualityservice.Model.ProductionOrderApi;
using qualityservice.Service.Interface;

namespace qualityservice.Service {
    public class CalculateAnalysisService : ICalculateAnalysisService {
        private readonly IConfiguration _configuration;
        private readonly IProductionOrderQualityService _productionOrderQualityService;
        private HttpClient client;

        public CalculateAnalysisService (IConfiguration configuration, IProductionOrderQualityService productionOrderQualityService) {
            _configuration = configuration;
            _productionOrderQualityService = productionOrderQualityService;
            client = new HttpClient ();
        }

        public async Task<List<MessageCalculates>> Calculates (int productionOrderId, double furnaceQuantity, Analysis analysis, bool ajuste) {
            List<MessageCalculates> messages = new List<MessageCalculates> ();
            List<string> stringReturn = new List<string> ();
            var productionOrder = await GetProductionOrder (productionOrderId);

            if (productionOrder == null) {
                MessageCalculates message = new MessageCalculates ();
                message.key = "Ordem não encontrada";
                message.value = "Ordem não encontrada";
                messages.Add (message);
            } else {

                var productionOrderQuality = await _productionOrderQualityService.GetProductionOrder (productionOrder.productionOrderId);
                if (productionOrderQuality == null)
                    productionOrderQuality = await _productionOrderQualityService.AddProductionOrderQuality (productionOrder);

                messages.AddRange (await CalculatesAnalysis (analysis, productionOrder, furnaceQuantity, productionOrderQuality, ajuste));

                if (!ajuste) {
                    productionOrderQuality.calculateInitial = messages;
                    await _productionOrderQualityService.updateProductionOrderQuality (productionOrderQuality.productionOrderQualityId, productionOrderQuality);
                }
            }
            return messages;
        }

        public async Task<List<MessageCalculates>> Recalculates (int productionOrderId,int productId, double quantityInput )
        {
            var productionOrder = await GetProductionOrder (productionOrderId);
            
            var productRecipe = productionOrder.recipe.phases.FirstOrDefault ()
                    .phaseProducts
                    .Where (x => x.product.productId == productId).FirstOrDefault ();

            
            var productionOrderQuality = await _productionOrderQualityService.GetProductionOrder (productionOrderId);

            var messagesProductionOrder = productionOrderQuality.calculateInitial;

            if(productRecipe.phaseProductType.ToLower() == "scrap")
            {
                var quantityMessageTotal = messagesProductionOrder.Sum(a=> Convert.ToDouble(a.value));

                foreach(var message in messagesProductionOrder)
                {
                    var productRecipeDebit = productionOrder.recipe.phases.FirstOrDefault ()
                    .phaseProducts
                    .Where (x => x.product.productId == message.productId).FirstOrDefault ();

                    double percentProduct = 0;
                    if(productRecipeDebit.phaseProductType.ToLower() == "base_product")
                        percentProduct = productRecipeDebit.maxValue;
                    else
                        percentProduct = productRecipeDebit.minValue;

                    var quantityDebit = quantityInput * (percentProduct / 100);
                    message.value = Math.Round((Convert.ToDouble(message.value) - quantityDebit),2).ToString();
                }

            }
            else
            {
                var productMessage = messagesProductionOrder.Where(a=>a.productId == productId).FirstOrDefault();

                if(productMessage != null)
                    productMessage.value = Math.Round((Convert.ToDouble(productMessage.value) - quantityInput),2).ToString();
            }

            await _productionOrderQualityService.updateProductionOrderQuality (productionOrderQuality.productionOrderQualityId, productionOrderQuality);

            return messagesProductionOrder;

        }
        private async Task<List<MessageCalculates>> CalculatesAnalysis (Analysis analysisReal, ProductionOrder productionOrder,
            double furnaceQuantity, ProductionOrderQuality productionOrderQuality, bool ajuste) {

            List<MessageCalculates> messages = new List<MessageCalculates> ();
            List<string> stringReturn = new List<string> ();
            double maxForno = Convert.ToDouble (_configuration["maxForno"]);
            double porcentagemForno = Convert.ToDouble (_configuration["porcentagemForno"]);
            int cobreFosforosoId = Convert.ToInt32 (_configuration["cobreFosforosoId"]);
            AnalysisComp analysisCobreEstimada = new AnalysisComp ();
            double qtdForno = 0;

            if (!ajuste)
                maxForno = maxForno * (porcentagemForno / 100);

            // Removendo cobre fosforoso dos calculos PONTO 1/2
            var cobreFosforosoComp = analysisReal.comp.Where (x => x.productId == cobreFosforosoId).FirstOrDefault ();
            if (cobreFosforosoComp != null)
                analysisReal.comp.Remove (cobreFosforosoComp);
            // Fim da remoção 1/2

            foreach (var compAnalysis in analysisReal.comp) {
                var componenteRecipe = productionOrder.recipe.phases.FirstOrDefault ()
                    .phaseProducts
                    .Where (x => x.product.productId == compAnalysis.productId).FirstOrDefault ();

                if (componenteRecipe == null)
                    compAnalysis.type = "contaminent";
                else
                    compAnalysis.type = componenteRecipe.phaseProductType.ToLower ();

                compAnalysis.valueKg = (compAnalysis.value / 100) * furnaceQuantity;
            }

            List<AnalysisComp> analysisRecipeList = new List<AnalysisComp> ();
            foreach (var compRecipe in productionOrder.recipe.phases.FirstOrDefault ().phaseProducts) {
                // Removendo cobre fosforoso dos calculos PONTO 2/2
                if (compRecipe.product.productId == cobreFosforosoId)
                    continue;
                // Fim da remoção 2/2

                // Retirando contaminantes
                if(compRecipe.phaseProductType.ToLower() == "contaminent" || compRecipe.phaseProductType.ToLower () == "scrap" || compRecipe.phaseProductType.ToLower() == "semi_finished")
                {
                    continue;
                }

                AnalysisComp analysisRecipe = new AnalysisComp ();
                analysisRecipe.productId = compRecipe.product.productId;
                analysisRecipe.productName = compRecipe.product.productName;
                analysisRecipe.value = ((compRecipe.maxValue - compRecipe.minValue) / 2) + compRecipe.minValue;
                analysisRecipe.valueKg = (analysisRecipe.value / 100) * maxForno;

                analysisRecipeList.Add (analysisRecipe);
            }

            List<AnalysisComp> analysisEstimadaList = new List<AnalysisComp> ();

            qtdForno = analysisReal.comp.Where (a => a.type == "contaminent").Sum (a => a.valueKg);
            qtdForno = analysisReal.comp.Where (a => a.type == "scrap").Sum (a => a.valueKg);

            foreach (var comp in analysisRecipeList) {

                AnalysisComp analysisEstimada = new AnalysisComp ();
                analysisEstimada.productId = comp.productId;
                analysisEstimada.productName = comp.productName;

                AnalysisComp compAnalysis = analysisReal.comp.Where (x => x.productId == comp.productId).FirstOrDefault ();

                if (compAnalysis == null) {
                    compAnalysis = new AnalysisComp ();
                    compAnalysis.productId = comp.productId;
                    compAnalysis.productName = comp.productName;
                    compAnalysis.value = 0;
                    compAnalysis.valueKg = 0;
                }

                analysisEstimada.value = (comp.valueKg / maxForno) * 100;

                analysisEstimada.valueKg = comp.valueKg - compAnalysis.valueKg;

                if (analysisEstimada.valueKg < 0)
                    qtdForno = qtdForno + (analysisEstimada.valueKg * -1) + compAnalysis.valueKg;
                else {
                    qtdForno = qtdForno + analysisEstimada.valueKg + compAnalysis.valueKg;
                }

                Console.WriteLine ("estimada " + analysisEstimada.valueKg);
                Console.WriteLine ("analise " + compAnalysis.valueKg);
                Console.WriteLine ("Forno " + qtdForno);
                if (qtdForno > maxForno) {
                    messages = new List<MessageCalculates> ();
                    MessageCalculates message = new MessageCalculates ();
                    message.key = "Excesso de Carga!";
                    message.value = "0";
                    message.productId = 0;
                    messages.Add (message);
                    return messages;
                }
                //productionOrderQuality.qntForno = qtdForno;
                if (analysisEstimada.valueKg > 0) {
                    MessageCalculates message = new MessageCalculates ();
                    message.key = analysisEstimada.productName;
                    message.productId = analysisEstimada.productId;
                    message.value = analysisEstimada.valueKg.ToString ();

                    messages.Add (message);
                }
            }
            await _productionOrderQualityService.updateProductionOrderQuality (productionOrderQuality.productionOrderQualityId, productionOrderQuality);

            return messages;
        }

        private async Task<ProductionOrder> GetProductionOrder (int productionOrderId) {
            var builder = new UriBuilder (_configuration["ProductionOrderApi"] + productionOrderId);
            string url = builder.ToString ();
            var result = await client.GetAsync (url);

            if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.NoContent)
                return JsonConvert.DeserializeObject<ProductionOrder> (await client.GetStringAsync (url));

            Console.WriteLine ("GetProductionOrder - status code != 202 e 204");
            return null;
        }
    }
}