using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using internet_shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;

namespace internet_shop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ShoppingContext _context;

        public OrdersController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder)
        {
            var user = User.Identity.Name;

            ViewData["DateSort"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (User.IsInRole("admin"))
            {
                var orders = from o in _context.Orders.Include(o => o.Customer).Include(o => o.Product) select o;

                switch (sortOrder)
                {
                    case "Date":
                        orders = orders.OrderBy(o => o.OrderDate);
                        break;
                    case "date_desc":
                        orders = orders.OrderByDescending(o => o.OrderDate);
                        break;
                }

                return View(await orders.AsNoTracking().ToListAsync());
            }
            else if (User.IsInRole("buyer"))
            {
                var orders = from o in _context.Orders.Include(o => o.Customer).Include(o => o.Product)
                             .Where(o => o.Customer.Email == user)
                             select o;

                switch (sortOrder)
                {
                    case "Date":
                        orders = orders.OrderBy(o => o.OrderDate);
                        break;
                    case "date_desc":
                        orders = orders.OrderByDescending(o => o.OrderDate);
                        break;
                }

                return View(await orders.AsNoTracking().ToListAsync());
            }

            return NotFound();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = User.Identity.Name;

            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            if (User.IsInRole("admin"))
            {
                var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            else if (User.IsInRole("buyer"))
            {
                var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id && m.Customer.Email == user);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }

            return NotFound();
        }

        // GET: Orders/Create
        public IActionResult Create(int productId)
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name", productId);
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,CustomerId,ProductId,Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Customers.FirstOrDefault(c => c.Email == User.Identity.Name);
                var product = _context.Products.FirstOrDefault(p => p.ID == order.ProductId);

                if (order.Quantity <= product.Count && order.Quantity > 0)
                {
                    order.OrderDate = DateTime.Now;
                    order.CustomerId = user.ID;

                    _context.Add(order);
                    await _context.SaveChangesAsync();

                    product.Count -= order.Quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Quantity", "The quantity requested is not available.");
                }
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name", order.ProductId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ID", order.ProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,CustomerId,ProductId,Quantity")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ID", order.ProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ShoppingContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
