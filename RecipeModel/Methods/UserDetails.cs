using System;
using System.Collections.Generic;
using System.Text;
using RecipeModel.Models;
using RecipeModel.Data;
using System.Security.Claims;
using System.Linq;

namespace RecipeModel.Methods
{
    public static class UserDetails
    {
        public static string GetName(this ClaimsPrincipal principal)
        {
            var name = principal.Claims.FirstOrDefault(c => c.Type == "Name");
            return name?.Value;
        }
    }
}
