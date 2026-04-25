using Website.Models;
using Website.Models.Repositories;
using Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace Website.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : BaseController
    {
        readonly IProductRepository ProductRepository;
        readonly ICategoryRepository CategRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductController(
            IProductRepository ProdRepository,
            ICategoryRepository categRepository,
            IWebHostEnvironment hostingEnvironment)
            : base(categRepository)
        {
            ProductRepository = ProdRepository;
            CategRepository = categRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]  // ← moved here from the deleted Index
        public IActionResult Index(int? categoryId, int page = 1)
        {
            int pageSize = 4;

            IQueryable<Product> productsQuery = ProductRepository.GetAllProducts();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
            }

            var totalProducts = productsQuery.Count();
            var products = productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;

            return View(products);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(
                CategRepository.GetAll(),
                "CategoryId",
                "CategoryName"
            );

            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.ImagePath != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    // Make sure the images folder exists
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImagePath.CopyTo(stream);
                    }
                }

                Product newProduct = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    QteStock = model.QteStock,
                    CategoryId = model.CategoryId.Value,  // .Value to convert int? -> int
                    Image = uniqueFileName
                };

                ProductRepository.Add(newProduct);

                return RedirectToAction("Index");
            }

            // Repopulate dropdown on validation failure
            ViewBag.CategoryId = new SelectList(
                CategRepository.GetAll(),
                "CategoryId",
                "CategoryName"
            );

            return View(model);
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            Product product = ProductRepository.GetById(id);

            ViewBag.CategoryId = new SelectList(
                CategRepository.GetAll(),
                "CategoryId",
                "CategoryName",
                product.CategoryId
            );

            EditViewModel vm = new EditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                QteStock = product.QteStock,
                CategoryId = product.CategoryId,
                ExistingImagePath = product.Image
            };

            return View(vm);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = ProductRepository.GetById(model.ProductId);

                product.Name = model.Name;
                product.Price = model.Price;
                product.QteStock = model.QteStock;
                product.CategoryId = model.CategoryId;  // int -> int, no cast needed

                string uploadedImage = ProcessUploadedFile(model);

                if (uploadedImage != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(model.ExistingImagePath))
                    {
                        string oldPath = Path.Combine(
                            hostingEnvironment.WebRootPath,
                            "images",
                            model.ExistingImagePath);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    product.Image = uploadedImage;
                }

                ProductRepository.Update(product);

                return RedirectToAction("Index");
            }

            // Repopulate dropdown on validation failure
            ViewBag.CategoryId = new SelectList(
                CategRepository.GetAll(),
                "CategoryId",
                "CategoryName"
            );

            return View(model);
        }

        // UPLOAD HELPER
        [NonAction]
        private string ProcessUploadedFile(EditViewModel model)
        {
            if (model.ImagePath != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                // Make sure the images folder exists
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(stream);
                }

                return uniqueFileName;
            }

            return null;
        }
    }
}