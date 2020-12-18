using System;
using System.Collections.Generic;
using System.Text;
using RecipeModel.Models;

namespace RecipeModel.ViewModel
{
    public class RecipeDetails
    {
        public Recipe Recipe { get; set; }
        public Image Image { get; set; }
        public AppUser AppUser { get; set; }
        public RecipeIngredient RecipeIngredient { get; set; }
        public Ingredient Ingredient { get; set; }
        public Brand Brand { get; set; }
    }
}
