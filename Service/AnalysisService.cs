using qualityservice.Service.Interface;
using qualityservice.Model;
using qualityservice.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace qualityservice.Service
{
    public class AnalysisService :IAnalysisService
    {
        private readonly IProductionOrderQualityService _productionOrderQualityService;
        private readonly ApplicationDbContext _context;

        public AnalysisService(IProductionOrderQualityService productionOrderQualityService, ApplicationDbContext context)
        {
            _context = context;
            _productionOrderQualityService = productionOrderQualityService;
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

            productionQuality.Analysis.Add(analysis);

            await _context.SaveChangesAsync();

            return analysis;
        }
    }
}