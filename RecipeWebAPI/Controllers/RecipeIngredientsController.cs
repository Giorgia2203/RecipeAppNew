using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeModel.Data;
using RecipeModel.Models;
using RecipeModel.ViewModel;

namespace RecipeWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientsController : ControllerBase
    {
        private readonly RecipeContext _context;

        public RecipeIngredientsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: api/RecipeIngredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeIngredient>>> GetRecipeIngredients()
        {
            return await _context.RecipeIngredients.ToListAsync();
        }

        // GET: api/RecipeIngredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeIngredient>> GetRecipeIngredient(int id)
        {
            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);

            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return recipeIngredient;
        }

        // PUT: api/RecipeIngredients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeIngredient(int id, RecipeIngredient recipeIngredient)
        {
            if (id != recipeIngredient.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipeIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeIngredientExists(id, recipeIngredient.IngredientId))
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

        // POST: api/RecipeIngredients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RecipeIngredient>> PostRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            _context.RecipeIngredients.Add(recipeIngredient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RecipeIngredientExists(recipeIngredient.RecipeId, recipeIngredient.IngredientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRecipeIngredient", new { id = recipeIngredient.RecipeId }, recipeIngredient);
        }

        // DELETE: api/RecipeIngredients/5
        [HttpDelete("{recipeId}/{ingredientId}/{brandId}")]
        public async Task<ActionResult<RecipeIngredient>> DeleteRecipeIngredient(int recipeId, int ingredientId, int brandId)
        {
            var recipeIngredient = await _context.RecipeIngredients.FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.IngredientId == ingredientId && x.BrandId == brandId);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            _context.RecipeIngredients.Remove(recipeIngredient);
            await _context.SaveChangesAsync();

            return recipeIngredient;
        }

        private bool RecipeIngredientExists(int recipeId, int ingredientId)
        {
            return _context.RecipeIngredients.Any(e => e.RecipeId == recipeId && e.IngredientId == ingredientId);
        }


        [HttpGet("{recipeId}/{ingredientId}/{brandId}")]
        public async Task<ActionResult<RecipeIngredient>> GetIngredientBrand(int recipeId, int ingredientId, int brandId)
        {
            var recipeIngredient = await _context.RecipeIngredients.FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.IngredientId == ingredientId && x.BrandId == brandId);

            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(x=>x.Id == ingredientId);
            var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == brandId);

            recipeIngredient.Brand = brand;
            recipeIngredient.Ingredient = ingredient;

            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return recipeIngredient;
        }


        [HttpPut("{recipeId}/{ingredientId}/{brandId}")]
        public async Task<ActionResult<IngredientBrand>> PutIngredientBrandEdit(int recipeId, int ingredientId, int brandId, RecipeIngredient updatedRecipeIngredient)
        {
            var recipeIngredient = await _context.RecipeIngredients.FirstOrDefaultAsync(x=>x.RecipeId == recipeId && x.IngredientId == ingredientId && x.BrandId == brandId);
           
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            recipeIngredient.Measurement = updatedRecipeIngredient.Measurement;
            recipeIngredient.Amount = updatedRecipeIngredient.Amount;
            _context.Entry(recipeIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeIngredientExists(recipeId,ingredientId))
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
    }
}
