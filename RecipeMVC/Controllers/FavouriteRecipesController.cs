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
    public class FavouriteRecipesController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/FavouriteRecipes";


        public FavouriteRecipesController(RecipeContext context)
        {
            _context = context;
        }

        // GET: FavouriteRecipes
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var faveRecipes = JsonConvert.DeserializeObject<List<FavouriteRecipe>>(await response.Content.ReadAsStringAsync());

                return View(faveRecipes);
            }

            return NotFound();
        }

        // GET: FavouriteRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var favouriteRecipe = await _context.FavouriteRecipes
                .Include(f => f.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var faceRecipes = JsonConvert.DeserializeObject<FavouriteRecipe>(await response.Content.ReadAsStringAsync());
                return View(faceRecipes);
            }

            return NotFound();
        }

        // GET: FavouriteRecipes/Create
        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            return View();
        }

        // POST: FavouriteRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,RecipeId")] FavouriteRecipe favouriteRecipe)
        {
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", favouriteRecipe.RecipeId);
            if (!ModelState.IsValid)
            {
                return View(favouriteRecipe);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(favouriteRecipe);
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

            return View(favouriteRecipe);
        }

        // GET: FavouriteRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", favouriteRecipe.RecipeId);
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var faveRecipe = JsonConvert.DeserializeObject<FavouriteRecipe>(await response.Content.ReadAsStringAsync());
                return View(faveRecipe);
            }

            return new NotFoundResult();
        }

        // POST: FavouriteRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RecipeId")] FavouriteRecipe favouriteRecipe)
        {
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", favouriteRecipe.RecipeId);
            if (!ModelState.IsValid) return View(favouriteRecipe);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(favouriteRecipe);
            var response = await client.PutAsync($"{_baseUrl}/{favouriteRecipe.Id}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(favouriteRecipe);
        }

        // GET: FavouriteRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*var favouriteRecipe = await _context.FavouriteRecipes
                .Include(f => f.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);*/
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var faveRecipe = JsonConvert.DeserializeObject<FavouriteRecipe>(await response.Content.ReadAsStringAsync());
                return View(faveRecipe);
            }

            return new NotFoundResult();
        }

        // POST: FavouriteRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] FavouriteRecipe favouriteRecipe)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{favouriteRecipe.Id}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(favouriteRecipe), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(favouriteRecipe);
        }
    }
}
