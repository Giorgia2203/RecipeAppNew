using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeModel.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BrandIngredient> Brands { get; set; }
        public ICollection<RecipeIngredient> Recipes { get; set; }
    }
}
