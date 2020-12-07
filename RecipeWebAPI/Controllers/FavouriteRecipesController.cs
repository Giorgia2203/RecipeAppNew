using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<FavouriteRecipe>> GetFavouriteRecipe(int id)
        {
            var favouriteRecipe = await _context.FavouriteRecipes.FindAsync(id);

            if (favouriteRecipe == null)
            {
                return NotFound();
            }

            return favouriteRecipe;
        }

        // PUT: api/FavouriteRecipes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavouriteRecipe(int id, FavouriteRecipe favouriteRecipe)
        {
            if (id != favouriteRecipe.Id)
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
                if (!FavouriteRecipeExists(id))
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
        public async Task<ActionResult<FavouriteRecipe>> PostFavouriteRecipe(FavouriteRecipe favouriteRecipe)
        {
            _context.FavouriteRecipes.Add(favouriteRecipe);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FavouriteRecipeExists(favouriteRecipe.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFavouriteRecipe", new { id = favouriteRecipe.Id }, favouriteRecipe);
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

        private bool FavouriteRecipeExists(int id)
        {
            return _context.FavouriteRecipes.Any(e => e.Id == id);
        }
    }
}
