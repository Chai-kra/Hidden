using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers;

public class CategoryController(DB db) : Controller
{
    public IActionResult Index()
    {
        var m = db.Categories;
        return View(m);
    }

    public bool CheckId(string id)
    {
        // No existing category with the given id
        return !db.Categories.Any(c => c.Id == id);
    }

    public IActionResult Insert()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Insert(CategoryVM vm)
    {
        if (ModelState.IsValid("Id") &&
            db.Categories.Any(c => c.Id == vm.Id))
        {
            ModelState.AddModelError("Id", "Duplicated Id.");
        }

        if (ModelState.IsValid)
        {
            db.Categories.Add(new()
            {
                Id = vm.Id,
                Name = vm.Name,
            });
            db.SaveChanges();

            TempData["Info"] = "Record inserted.";
            return RedirectToAction("Index");
        }

        return View();
    }

    public IActionResult Update(string? id)
    {
        var c = db.Categories.Find(id);

        if (c == null)
        {
            return RedirectToAction("Index");
        }

        var vm = new CategoryVM
        {
            Id = c.Id,
            Name = c.Name,
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Update(CategoryVM vm)
    {
        var c = db.Categories.Find(vm.Id);

        if (c == null)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            c.Name = vm.Name;
            db.SaveChanges();

            TempData["Info"] = "Record Updated.";
            return RedirectToAction("Index");
        }

        return View(vm);
    }

    [HttpPost]
    public IActionResult Delete(string? Id)
    {
        db.Categories
          .Where(c => c.Id == Id)
          .ExecuteDelete();

        TempData["Info"] = "Record deleted.";
        return RedirectToAction("Index");
    }
}
