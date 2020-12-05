using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeModel.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BrandIngredient> Ingredients { get; set; }
    }
}
