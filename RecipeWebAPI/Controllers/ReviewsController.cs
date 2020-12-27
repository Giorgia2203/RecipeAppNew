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
    public class ReviewsController : ControllerBase
    {
        private readonly RecipeContext _context;

        public ReviewsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{recipeId}")]
        public async Task<ActionResult<double>> GetReview(int recipeId)
        {
            var ratings = await _context.Reviews.Where(x => x.RecipeId == recipeId).Select(x => new { x.Rating }).ToListAsync(); ;
            double average = 0;

            if (ratings.Count() > 0)
            {
                average = (double)ratings.Sum(x => x.Rating) / ratings.Count();
                return Math.Round(average, 2);
            }

            return 0;
        }

        // GET: api/Review/5/user-hash
        [HttpGet("{recipeId}/{userId}")]
        public async Task<ActionResult<Review>> GetReviewByUserIdAndRecipeId(int recipeId, string userId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.AppUserId.Equals(userId));

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{userId}/{recipeId}")]
        public async Task<IActionResult> PutReview(string userId, int recipeId, Review review)
        {
            if (userId != review.AppUserId || recipeId != review.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(userId, recipeId))
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

        // POST: api/Reviews
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ReviewExists(review.AppUserId, review.RecipeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetReview", new { appUserId = review.AppUserId, recipeId = review.RecipeId }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }

        private bool ReviewExists(string userId, int recipeId)
        {
            return _context.Reviews.Any(e => e.AppUserId.Equals(userId) && e.RecipeId == recipeId);
        }
    }
}
