using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers;

public class ProductController(DB db) : Controller
{
    public IActionResult Index()
    {
        var m = db.Products;
        return View(m);
    }

    public bool CheckId(string id)
    {
        // No existing product with the given id
        return !db.Products.Any(p => p.Id == id);
    }

    public bool CheckCategoryId(string categoryId)
    {
        // Has existing category with the given id
        return db.Categories.Any(c => c.Id == categoryId);
    }

    public IActionResult Insert()
    {
        ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult Insert(ProductVM vm)
    {
        if (ModelState.IsValid("Id") &&
            db.Products.Any(p => p.Id == vm.Id))
        {
            ModelState.AddModelError("Id", "Duplicated Id.");
        }

        if (ModelState.IsValid("CategoryId") &&
            !db.Categories.Any(c => c.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Invalid Category.");
        }

        if (ModelState.IsValid)
        {
            db.Products.Add(new()
            {
                Id = vm.Id,
                Name = vm.Name,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
            });
            db.SaveChanges();

            TempData["Info"] = "Record inserted.";
            return RedirectToAction("Index");
        }

        ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");
        return View();
    }

    public IActionResult Update(string? id)
    {
        var p = db.Products.Find(id);

        if (p == null)
        {
            return RedirectToAction("Index");
        }

        var vm = new ProductVM
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CategoryId = p.CategoryId,
        };

        ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");
        return View(vm);
    }

    [HttpPost]
    public IActionResult Update(ProductVM vm)
    {
        var p = db.Products.Find(vm.Id);

        if (p == null)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid("CategoryId") &&
            !db.Categories.Any(c => c.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Invalid Category.");
        }

        if (ModelState.IsValid)
        {
            p.Name = vm.Name;
            p.Price = vm.Price;
            p.CategoryId = vm.CategoryId;
            db.SaveChanges();

            TempData["Info"] = "Record updated.";
            return RedirectToAction("Index");
        }

        ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");
        return View(vm);
    }

    [HttpPost]
    public IActionResult Delete(string? Id)
    {
        db.Products
          .Where(p => p.Id == Id)
          .ExecuteDelete();

        TempData["Info"] = "Record deleted.";
        return RedirectToAction("Index");
    }
}
