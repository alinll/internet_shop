using internet_shop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using internet_shop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace internet_shop.Controllers
{
    public class AccountController : Controller
    {
        private ShoppingContext db;

        public AccountController(ShoppingContext context)
        {
            db = context;
        }

        private async Task Authenticate(Customer customer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, customer.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, customer.Role.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        Address = model.Address,
                        Email = model.Email,
                        Password = model.Password,
                        RoleId = 2
                    };

                    Role customerRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "buyer");
                    if (customerRole != null)
                    {
                        customer.Role = customerRole;
                    }

                    db.Customers.Add(customer);
                    await db.SaveChangesAsync();
                    await Authenticate(customer);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Incorrect login and(or) password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await db.Customers.Include(c => c.Role).FirstOrDefaultAsync(c => c.Email == model.Email
                && c.Password == model.Password);

                if (customer != null)
                {
                    await Authenticate(customer);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and(or) password");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
