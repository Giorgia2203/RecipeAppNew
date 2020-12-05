using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RecipeModel.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<FavouriteRecipe> FaveRecipe { get; set; }
        public ICollection<Image> Image { get; set; }
    }
}
