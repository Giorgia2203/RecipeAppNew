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
    public class BrandIngredientsController : ControllerBase
    {
        private readonly RecipeContext _context;

        public BrandIngredientsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: api/BrandIngredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandIngredient>>> GetBrandIngredients()
        {
            return await _context.BrandIngredients.ToListAsync();
        }

        // GET: api/BrandIngredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandIngredient>> GetBrandIngredient(int id)
        {
            var brandIngredient = await _context.BrandIngredients.FindAsync(id);

            if (brandIngredient == null)
            {
                return NotFound();
            }

            return brandIngredient;
        }

        // PUT: api/BrandIngredients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrandIngredient(int id, BrandIngredient brandIngredient)
        {
            if (id != brandIngredient.BrandId)
            {
                return BadRequest();
            }

            _context.Entry(brandIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandIngredientExists(id))
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

        // POST: api/BrandIngredients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BrandIngredient>> PostBrandIngredient(BrandIngredient brandIngredient)
        {
            _context.BrandIngredients.Add(brandIngredient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BrandIngredientExists(brandIngredient.BrandId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBrandIngredient", new { id = brandIngredient.BrandId }, brandIngredient);
        }

        // DELETE: api/BrandIngredients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BrandIngredient>> DeleteBrandIngredient(int id)
        {
            var brandIngredient = await _context.BrandIngredients.FindAsync(id);
            if (brandIngredient == null)
            {
                return NotFound();
            }

            _context.BrandIngredients.Remove(brandIngredient);
            await _context.SaveChangesAsync();

            return brandIngredient;
        }

        private bool BrandIngredientExists(int id)
        {
            return _context.BrandIngredients.Any(e => e.BrandId == id);
        }
    }
}
