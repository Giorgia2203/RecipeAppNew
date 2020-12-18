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
    public class ImagesController : Controller
    {
        private readonly RecipeContext _context;
        private string _baseUrl = "http://localhost:50541/api/Images";


        public ImagesController(RecipeContext context)
        {
            _context = context;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var recipeContext = _context.Images.Include(i => i.Recipe);
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var images = JsonConvert.DeserializeObject<List<Image>>(await response.Content.ReadAsStringAsync());

                return View(images);
            }

            return NotFound();
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
           /*var image = await _context.Images
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);*/
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var images = JsonConvert.DeserializeObject<Image>(await response.Content.ReadAsStringAsync());
                return View(images);
            }

            return NotFound();

        }

        // GET: Images/Create
        public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecipeId,UserId,Description,Filepath")] Image image)
        {
            if (!ModelState.IsValid)
            {
                return View(image);
            }

            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(image);
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

            return View(image);
        }

        // GET: Images/Edit/5
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
                var image = JsonConvert.DeserializeObject<Image>(await response.Content.ReadAsStringAsync());
                return View(image);
            }

            return new NotFoundResult();
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeId,UserId,Description,Filepath")] Image image)
        {
            if (!ModelState.IsValid) return View(image);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(image);
            var response = await client.PutAsync($"{_baseUrl}/{image.Id}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(image);
        }

        // GET: Images/Delete/5
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
                var image = JsonConvert.DeserializeObject<Image>(await response.Content.ReadAsStringAsync());
                return View(image);
            }

            return new NotFoundResult();

        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id")] Image image)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{image.Id}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(image), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record:{ex.Message}");
            }

            return View(image);
        }
    }
}