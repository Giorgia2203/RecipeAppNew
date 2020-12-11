using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeModel.Models
{
    public class Image
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }
        public string Filepath { get; set; }

        public Recipe Recipe { get; set; }
        public AppUser AppUser { get; set; }
    }
}
