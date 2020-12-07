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
    public class IngredientsController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/Ingredients";

        public IngredientsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(await response.Content.ReadAsStringAsync());

                return View(ingredients);
            }

            return NotFound();
        }

        // GET: Ingredients/Details/5
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
                var ingredients = JsonConvert.DeserializeObject<Ingredient>(await response.Content.ReadAsStringAsync());
                return View(ingredients);
            }

            return NotFound();
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                return View(ingredient);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(ingredient);
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

            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
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
                var ingredient = JsonConvert.DeserializeObject<Ingredient>(await response.Content.ReadAsStringAsync());
                return View(ingredient);
            }

            return new NotFoundResult();
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Ingredient ingredient)
        {
            if (!ModelState.IsValid) return View(ingredient);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(ingredient);
            var response = await client.PutAsync($"{_baseUrl}/{ingredient.Id}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
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
                var ingredient = JsonConvert.DeserializeObject<Ingredient>(await response.Content.ReadAsStringAsync());
                return View(ingredient);
            }

            return new NotFoundResult();
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] Ingredient ingredient)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{ingredient.Id}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(ingredient), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(ingredient);
        }
    }
}
