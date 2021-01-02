using System;
using System.Collections.Generic;
using System.Text;
using RecipeModel.Models;
using RecipeModel.Data;

namespace RecipeModel.ViewModel
{
    public class IngredientBrand
    {
        public Ingredient Ingredient { get; set; }
        public Brand Brand { get; set; }
        public RecipeIngredient RecipeIngredient {get; set;}
    }
}
