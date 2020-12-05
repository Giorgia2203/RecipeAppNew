using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeModel.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public int Amount { get; set; }
        public string Measurement { get; set; }

        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
