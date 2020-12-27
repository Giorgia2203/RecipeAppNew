using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeModel.Data;
using RecipeModel.Models;

namespace RecipeWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteRecipesController : ControllerBase
    {
        private readonly RecipeContext _context;

        public FavouriteRecipesController(RecipeContext context)
        {
            _context = context;
        }

        // GET: api/FavouriteRecipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteRecipe>>> GetFavouriteRecipes()
        {
            return await _context.FavouriteRecipes.ToListAsync();
        }

        // GET: api/FavouriteRecipes/5
        [HttpGet("{userId}/{recipeId}")]
        public async Task<ActionResult<FavouriteRecipe>> GetFavouriteRecipe(string userId, int recipeId)
        {
            var favouriteRecipe = await _context.FavouriteRecipes.FirstOrDefaultAsync(x => x.AppUserId.Equals(userId) && x.RecipeId == recipeId);

            if (favouriteRecipe == null)
            {
                return NotFound();
            }

            return favouriteRecipe;
        }

        // PUT: api/FavouriteRecipes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{userId}/{recipeId}")]
        public async Task<IActionResult> PutFavouriteRecipe(string userId,int recipeId, FavouriteRecipe favouriteRecipe)
        {
            if (userId != favouriteRecipe.AppUserId || recipeId != favouriteRecipe.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(favouriteRecipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteRecipeExists(userId, recipeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FavouriteRecipes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<FavouriteRecipe>> PostFavouriteRecipe(FavouriteRecipe favouriteRecipe)
        {
            _context.FavouriteRecipes.Add(favouriteRecipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavouriteRecipe", new { userId = favouriteRecipe.AppUserId, recipeId = favouriteRecipe.RecipeId }, favouriteRecipe);
        }

        // DELETE: api/FavouriteRecipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FavouriteRecipe>> DeleteFavouriteRecipe(int id)
        {
            var favouriteRecipe = await _context.FavouriteRecipes.FindAsync(id);
            if (favouriteRecipe == null)
            {
                return NotFound();
            }

            _context.FavouriteRecipes.Remove(favouriteRecipe);
            await _context.SaveChangesAsync();

            return favouriteRecipe;
        }

        private bool FavouriteRecipeExists(string userId, int recipeId)
        {
            return _context.FavouriteRecipes.Any(e => e.AppUserId.Equals(userId) && e.RecipeId == recipeId);
        }
    }
}
