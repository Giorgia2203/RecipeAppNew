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
    public class RecipeIngredientsController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/RecipeIngredients";

        public RecipeIngredientsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: RecipeIngredients
        public async Task<IActionResult> Index()
        {
            var recipeContext = _context.RecipeIngredients.Include(r => r.Ingredient).Include(r => r.Recipe);
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var recipeingredients = JsonConvert.DeserializeObject<List<RecipeIngredient>>(await response.Content.ReadAsStringAsync());

                return View(recipeingredients);
            }

            return NotFound();
        }

        // GET: RecipeIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*var recipeIngredient = await _context.RecipeIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeId == id);*/
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var recipeingredients = JsonConvert.DeserializeObject<RecipeIngredient>(await response.Content.ReadAsStringAsync());
                return View(recipeingredients);
            }

            return NotFound();
        }

        // GET: RecipeIngredients/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id");
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            return View();
        }

        // POST: RecipeIngredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,IngredientId,Amount,Measurement")] RecipeIngredient recipeIngredient)
        {
            //ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", recipeIngredient.IngredientId);
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", recipeIngredient.RecipeId);
            if (!ModelState.IsValid)
            {
                return View(recipeIngredient);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(recipeIngredient);
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

            return View(recipeIngredient);
        }

        // GET: RecipeIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", recipeIngredient.IngredientId);
            // ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", recipeIngredient.RecipeId);
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var recipeingredient = JsonConvert.DeserializeObject<RecipeIngredient>(await response.Content.ReadAsStringAsync());
                return View(recipeingredient);
            }

            return new NotFoundResult();
        }

        // POST: RecipeIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,IngredientId,Amount,Measurement")] RecipeIngredient recipeIngredient)
        {
            // ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", recipeIngredient.IngredientId);
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", recipeIngredient.RecipeId);
            if (!ModelState.IsValid) return View(recipeIngredient);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(recipeIngredient);
            var response = await client.PutAsync($"{_baseUrl}/{recipeIngredient.RecipeId/recipeIngredient.IngredientId}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(recipeIngredient);
        }

        // GET: RecipeIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*var recipeIngredient = await _context.RecipeIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeId == id);*/
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var recipengredient = JsonConvert.DeserializeObject<RecipeIngredient>(await response.Content.ReadAsStringAsync());
                return View(recipengredient);
            }

            return new NotFoundResult();
        }

        // POST: RecipeIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] RecipeIngredient recipeIngredient)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{recipeIngredient.RecipeId}/{recipeIngredient.IngredientId}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(recipeIngredient), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(recipeIngredient);
        }
    }
}
