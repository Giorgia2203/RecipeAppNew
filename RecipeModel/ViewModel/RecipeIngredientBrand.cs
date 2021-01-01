using System;
using System.Collections.Generic;
using System.Text;
using RecipeModel.Models;

namespace RecipeModel.ViewModel
{
    public class RecipeIngredientBrand
    {
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
        public Brand Brand { get; set; }
        public RecipeIngredient RecipeIngredient { get; set; }
        public BrandIngredient BrandIngredient { get; set; }
    }
}
