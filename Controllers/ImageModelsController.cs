using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UploadImage.Models;
using UploadImage.Models.Data;

namespace UploadImage.Controllers
{
    public class ImageModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageModelsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: ImageModels
        public async Task<IActionResult> Index()
        {
            return _context.Images != null ?
                        View(await _context.Images.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Images'  is null.");
        }

        // GET: ImageModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // GET: ImageModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImageModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ImageFile")] ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extensions = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.Name = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extensions;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }


                _context.Add(imageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // GET: ImageModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            return View(imageModel);
        }

        // POST: ImageModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ImageFile")] ImageModel imageModel)
        {
            if (id != imageModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", imageModel.Name);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Replace(imagePath,"","");

                    _context.Update(imageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(imageModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // GET: ImageModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // POST: ImageModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Images == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Images'  is null.");
            }
            var imageModel = await _context.Images.FindAsync(id);
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", imageModel.Name);

            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            _context.Images.Remove(imageModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageModelExists(int id)
        {
            return (_context.Images?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
