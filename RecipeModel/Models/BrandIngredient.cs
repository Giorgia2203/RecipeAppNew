using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeModel.Models
{
    public class BrandIngredient
    {
        public int BrandId { get; set; }

        public int IngredientId { get; set; }

        public Brand Brand { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
