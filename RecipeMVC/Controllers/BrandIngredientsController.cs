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
    public class BrandIngredientsController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/BrandIngredients";


        public BrandIngredientsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: BrandIngredients
        public async Task<IActionResult> Index()
        {
            var recipeContext = _context.BrandIngredients.Include(b => b.Brand).Include(b => b.Ingredient);
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var brandingredients = JsonConvert.DeserializeObject<List<BrandIngredient>>(await response.Content.ReadAsStringAsync());

                return View(brandingredients);
            }

            return NotFound();
        }

        // GET: BrandIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var brandIngredient = await _context.BrandIngredients
                .Include(b => b.Brand)
                .Include(b => b.Ingredient)
                .FirstOrDefaultAsync(m => m.BrandId == id);

            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var brandingredients = JsonConvert.DeserializeObject<BrandIngredient>(await response.Content.ReadAsStringAsync());
                return View(brandingredients);
            }

            return NotFound();
        }

        // GET: BrandIngredients/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id");
            return View();
        }

        // POST: BrandIngredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,IngredientId")] BrandIngredient brandIngredient)
        {
            //ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", brandIngredient.BrandId);
            //ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", brandIngredient.IngredientId);
            if (!ModelState.IsValid)
            {
                return View(brandIngredient);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(brandIngredient);
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

            return View(brandIngredient);
        }

        // GET: BrandIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            //ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", brandIngredient.BrandId);
            //ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", brandIngredient.IngredientId);
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var brandingredient = JsonConvert.DeserializeObject<BrandIngredient>(await response.Content.ReadAsStringAsync());
                return View(brandingredient);
            }

            return new NotFoundResult();
        }

        // POST: BrandIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,IngredientId")] BrandIngredient brandIngredient)
        {
            //ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", brandIngredient.BrandId);
            //ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", brandIngredient.IngredientId);

            if (!ModelState.IsValid) return View(brandIngredient);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(brandIngredient);
            var response = await client.PutAsync($"{_baseUrl}/{brandIngredient.BrandId}/{brandIngredient.IngredientId}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(brandIngredient);
        }

        // GET: BrandIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /* var brandIngredient = await _context.BrandIngredients
                  .Include(b => b.Brand)
                  .Include(b => b.Ingredient)
                  .FirstOrDefaultAsync(m => m.BrandId == id);*/
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if (response.IsSuccessStatusCode)
            {
                var brandingredient = JsonConvert.DeserializeObject<BrandIngredient>(await response.Content.ReadAsStringAsync());
                return View(brandingredient);
            }

            return new NotFoundResult();
        }

        // POST: BrandIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] BrandIngredient brandIngredient)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{brandIngredient.BrandId}/{brandIngredient.IngredientId}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(brandIngredient), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(brandIngredient);
        }
    }
}
