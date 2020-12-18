using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeModel.Data;
using RecipeModel.Models;
using RecipeModel.ViewModel;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;


namespace RecipeMVC.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/Recipes";

        public RecipesController(RecipeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexImage()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var recipes = JsonConvert.DeserializeObject<List<Recipe>>(await response.Content.ReadAsStringAsync());
                var images = JsonConvert.DeserializeObject<List<Image>>(await (await client.GetAsync("http://localhost:50541/api/Images")).Content.ReadAsStringAsync());

                var listRecImg = new List<RecipeImage>();

                foreach(var recipe in recipes)
                {
                    var image = images.Where(x => x.RecipeId == recipe.Id).ToList().First();

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

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var recipes = JsonConvert.DeserializeObject<List<Recipe>>(await response.Content.ReadAsStringAsync());

                return View(recipes);
            }

            return NotFound();
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var recipes = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
                return View(recipes);
            }

            return NotFound();
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Instructions,PreparationTime,BakingTime,ServingSize,Category,CreationDate")] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(recipe);
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

            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var recipe = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
                return View(recipe);
            }

            return new NotFoundResult();
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Instructions,PreparationTime,BakingTime,ServingSize,Category,CreationDate")] Recipe recipe)
        {
            if (!ModelState.IsValid) return View(recipe);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(recipe);
            var response = await client.PutAsync($"{_baseUrl}/{recipe.Id}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var recipe = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
                return View(recipe);
            }

            return new NotFoundResult();
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] Recipe recipe)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{recipe.Id}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(recipe), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(recipe);
        }

        public async Task<IActionResult> GetDessert()
        {
            
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}");

            if (response.IsSuccessStatusCode)
            {
                var recipes = JsonConvert.DeserializeObject<List<Recipe>>(await response.Content.ReadAsStringAsync());

                var recipe = recipes.Where(x => x.Category.Equals("Desert"));

                return View(recipe);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> GetMainDish()
        {

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}");

            if (response.IsSuccessStatusCode)
            {
                var recipes = JsonConvert.DeserializeObject<List<Recipe>>(await response.Content.ReadAsStringAsync());

                var recipe = recipes.Where(x => x.Category.Equals("Fel principal"));

                return View(recipe);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> GetSoup()
        {

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}");

            if (response.IsSuccessStatusCode)
            {
                var recipes = JsonConvert.DeserializeObject<List<Recipe>>(await response.Content.ReadAsStringAsync());

                var recipe = recipes.Where(x => x.Category.Equals("Supă"));

                return View(recipe);
            }

            return new NotFoundResult();
        }

        public async Task<IActionResult> RecImgDetails(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var recipe = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
                var images = JsonConvert.DeserializeObject<List<Image>>(await (await client.GetAsync("http://localhost:50541/api/Images")).Content.ReadAsStringAsync());

                var image = images.Where(x => x.RecipeId == id).ToList().First();
                var user = _context.AppUsers.Where(x => x.Id == recipe.AppUserId).ToList().First();

                RecipeDetails recImgDet = new RecipeDetails
                {
                    Recipe = recipe,
                    Image = image,
                    AppUser = user
                };

                return View(recImgDet);
            }

            return NotFound();
        }
    }
}
