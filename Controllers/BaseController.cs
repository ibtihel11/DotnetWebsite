using Website.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Website.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ICategoryRepository CategRepository;

        public BaseController(ICategoryRepository categRepository)
        {
            CategRepository = categRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var categories = CategRepository.GetAll();
            ViewData["Categories"] = categories;

            base.OnActionExecuting(context);
        }
    }
}