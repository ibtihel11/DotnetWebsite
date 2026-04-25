using Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        // ================= REGISTER =================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Product");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ================= LOGIN =================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Product");
            }

            ModelState.AddModelError("", "Invalid Login Attempt");
            return View(model);
        }

        // ================= LOGOUT =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        // ================= ACCESS DENIED =================

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ================= ROLE EDIT (GET) =================

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
                return View("NotFound");

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<UserRoleViewModel>()
            };

            var allUsers = userManager.Users.ToList();

            foreach (var user in allUsers)
            {
                model.Users.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(model);
        }

        // ================= ROLE EDIT (POST) =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
                return View("NotFound");

            role.Name = model.RoleName;

            var result = await roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            foreach (var userModel in model.Users)
            {
                var user = await userManager.FindByIdAsync(userModel.UserId);

                if (user == null)
                    continue;

                bool isInRole = await userManager.IsInRoleAsync(user, role.Name);

                if (userModel.IsSelected && !isInRole)
                    await userManager.AddToRoleAsync(user, role.Name);

                else if (!userModel.IsSelected && isInRole)
                    await userManager.RemoveFromRoleAsync(user, role.Name);
            }

            return RedirectToAction("ListRoles");
        }

        // ================= LIST ROLES =================

        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
    }
}