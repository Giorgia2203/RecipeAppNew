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
using RecipeModel.ViewModel;

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

        public async Task<IActionResult> GetFaveRecipes(string userId)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var faveRecipesAll = JsonConvert.DeserializeObject<List<FavouriteRecipe>>(await response.Content.ReadAsStringAsync());
                var images = JsonConvert.DeserializeObject<List<Image>>(await (await client.GetAsync("http://localhost:50541/api/Images")).Content.ReadAsStringAsync());

                var faveRecipesForUser = faveRecipesAll.Where(x => x.AppUserId.Equals(userId));

                var listRecImg = new List<RecipeImage>();

                foreach (var faveRecipe in faveRecipesForUser)
                {
                    var recipe = _context.Recipes.FirstOrDefault(x => x.Id == faveRecipe.RecipeId);
                    var image = images.FirstOrDefault(x => x.RecipeId == faveRecipe.RecipeId);

                    RecipeImage recipeImage = new RecipeImage
                    {
                        Recipe = recipe,
                        Image = image
                    };

                    listRecImg.Add(recipeImage);
                }

                return View(listRecImg);
            }

            return NotFound();

        }

        // GET: FavouriteRecipes/Details/5
        public async Task<IActionResult> Details(string? userId, int recipeId)
        {
            var favouriteRecipe = await _context.FavouriteRecipes
                .Include(f => f.Recipe)
                .FirstOrDefaultAsync(m => m.AppUserId.Equals(userId) && m.RecipeId == recipeId);
            if (userId == null || recipeId == 0)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{userId}/{recipeId}");
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
        public async Task<IActionResult> Create(FavouriteRecipe favouriteRecipe)
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
        public async Task<IActionResult> Edit(string? userId, int? recipeId)
        {
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", favouriteRecipe.RecipeId);
            if (String.IsNullOrEmpty(userId) || recipeId == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{userId}/{recipeId.Value}");

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
        public async Task<IActionResult> Edit(string userId, int recipeId, [Bind("UserId,RecipeId")] FavouriteRecipe favouriteRecipe)
        {
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", favouriteRecipe.RecipeId);
            if (!ModelState.IsValid) return View(favouriteRecipe);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(favouriteRecipe);
            var response = await client.PutAsync($"{_baseUrl}/{favouriteRecipe.AppUserId}/{favouriteRecipe.RecipeId}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(favouriteRecipe);
        }

        // GET: FavouriteRecipes/Delete/5
        public async Task<IActionResult> Delete(string? userId, int? recipeId)
        {
            /*var favouriteRecipe = await _context.FavouriteRecipes
                .Include(f => f.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);*/
            if (String.IsNullOrEmpty(userId) || recipeId == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{userId}/{recipeId.Value}");

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
        public async Task<IActionResult> DeleteConfirmed([Bind("AppUserId,RecipeId")] FavouriteRecipe favouriteRecipe)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{favouriteRecipe.AppUserId}/{favouriteRecipe.RecipeId}")
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
