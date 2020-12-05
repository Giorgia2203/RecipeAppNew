using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeModel.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int PreparationTime { get; set; }
        public int BakingTime { get; set; }
        public int ServingSize { get; set; }
        public string Category { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Review> Review { get; set; }
        public ICollection<FavouriteRecipe> FaveRecipe { get; set; }
        public ICollection<Image> Image { get; set; }
        public ICollection<RecipeIngredient> Ingrediente { get; set; }
    }
}
