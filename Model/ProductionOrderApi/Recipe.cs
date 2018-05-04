using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace qualityservice.Model.ProductionOrderApi
{
    public class Recipe
    {
        [Key]
        public int internalId { get; set; }
        public int recipeId { get; set; }
        [MaxLength(50)]
        public string recipeName { get; set; }
        public string recipeDescription { get; set; }
        [MaxLength(50)]
        public string recipeCode { get; set; }
        public ICollection<Phase> phases { get; set; }
    }
}