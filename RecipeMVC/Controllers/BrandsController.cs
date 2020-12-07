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
    public class BrandsController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/Brands";

        public BrandsController(RecipeContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var brands = JsonConvert.DeserializeObject<List<Brand>>(await response.Content.ReadAsStringAsync());

                return View(brands);
            }

            return NotFound();
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if(response.IsSuccessStatusCode)
            {
                var brands = JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
                return View(brands);
            }

            return NotFound();
            
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(brand);
                var response = await client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record:{ex.Message}");
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if(response.IsSuccessStatusCode)
            {
                var brand = JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
                return View(brand);
            }

            return new NotFoundResult();
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Brand brand)
        {
            if (!ModelState.IsValid) return View(brand);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(brand);
            var response = await client.PutAsync($"{_baseUrl}/{brand.Id}",new StringContent(json,Encoding.UTF8,"application/json"));

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");

            if(response.IsSuccessStatusCode)
            {
                var brand = JsonConvert.DeserializeObject<Brand>(await response.Content.ReadAsStringAsync());
                return View(brand);
            }

            return new NotFoundResult();
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] Brand brand)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{brand.Id}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(brand),Encoding.UTF8,"application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(brand);
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
