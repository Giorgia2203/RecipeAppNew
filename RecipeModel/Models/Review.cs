using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeModel.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public int Rating { get; set; }

        public AppUser AppUser { get; set; }
        public Recipe Recipe { get; set; }
    }
}
