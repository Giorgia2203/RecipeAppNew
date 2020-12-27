using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeModel.Data;
using RecipeModel.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace RecipeMVC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/Reviews";

        public ReviewsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var recipeContext = _context.Reviews.Include(r => r.Recipe);
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var reviews = JsonConvert.DeserializeObject<List<Review>>(await response.Content.ReadAsStringAsync());

                return View(reviews);
            }

            return NotFound();
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(string userId, int recipeId)
        {
            var review = await _context.Reviews
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.AppUserId.Equals(userId) && m.RecipeId==recipeId);
            if (userId == null || recipeId==0)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{userId}/{recipeId}");
            if (response.IsSuccessStatusCode)
            {
                var reviews = JsonConvert.DeserializeObject<Review>(await response.Content.ReadAsStringAsync());
                return View(reviews);
            }

            return NotFound();
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RecipeId,Rating")] Review review)
        {
            if (!ModelState.IsValid)
            {
                return View(review);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(review);
                var response = await client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record:{ex.Message}");
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(string userId, int recipeId)
        {
            if (userId == null || recipeId == 0)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{userId}/{recipeId}");

            if (response.IsSuccessStatusCode)
            {
                var review = JsonConvert.DeserializeObject<Review>(await response.Content.ReadAsStringAsync());
                return View(review);
            }

            return new NotFoundResult();
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, int recipeId, [Bind("UserId,RecipeId,Rating")] Review review)
        {
            if (!ModelState.IsValid) return View(review);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(review);
            var response = await client.PutAsync($"{_baseUrl}/{review.AppUserId}/{review.RecipeId}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(string userId, int recipeId)
        {
            if (userId == null || recipeId == 0)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{userId}/{recipeId}");

            if (response.IsSuccessStatusCode)
            {
                var review = JsonConvert.DeserializeObject<Review>(await response.Content.ReadAsStringAsync());
                return View(review);
            }

            return new NotFoundResult();
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("AppUserId, RecipeId")] Review review)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{review.AppUserId}/{review.RecipeId}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(review);
        }
    }
}
