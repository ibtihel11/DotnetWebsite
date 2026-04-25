using Website.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository CategRepository;

        public CategoryController(ICategoryRepository categRepository)
            : base(categRepository)
        {
            CategRepository = categRepository;
        }

        // GET: CategoryController
        [AllowAnonymous]
        public ActionResult Index()
        {
            var categories = CategRepository.GetAll();
            return View(categories);
        }
    }
}