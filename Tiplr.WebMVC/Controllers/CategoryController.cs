using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tiplr.Models;
using Tiplr.Services;

namespace Tiplr.WebMVC.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize]
        // GET: Category
        public ActionResult Index()
        {
            var svc = CreateCategoryService();
            var model = svc.GetCategories();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var svc = CreateCategoryService();
            if (svc.CreateProductCategory(model))
            {
                TempData["SaveResult"] = "Your product category was successfully created.";
                return RedirectToAction("Index");
            };
            ModelState.AddModelError("", "Category could not be created.");
            return View(model);
        }

        //Get Category/{id}
        public ActionResult Details(int id)
        {
            var svc = CreateCategoryService();
            var model = svc.GetCategoryById(id);
            return View(model);
        }

        //Get EDIT
        public ActionResult Edit(int id)
        {
            var svc = CreateCategoryService();
            var detail = svc.GetCategoryById(id);
            var model = new CategoryEdit
            {
                CategoryId = detail.CategoryId,
                CategoryName = detail.CategoryName,
                Active = detail.Active
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEdit model, int id)
        {
            if (!ModelState.IsValid) return View(model);

            if(model.CategoryId != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }

            var svc = CreateCategoryService();
            if (svc.UpdateCategory(model))
            {
                TempData["Save Result"] = "Category updated!";
                if (model.Active == false)
                {
                    if (UpdtInactiveCategoryProduct(id) > 0)
                    {
                        TempData["SaveResult"] = "Products with the inactivated category have been updated.";
                    }

                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Category could not be updated");
            return View(model);
        }

        //Helper Methods

        private int UpdtInactiveCategoryProduct(int id)
        {
            var catSvc = CreateCategoryService();
            var prodSvc = CreateProductService();
            var productList = catSvc.UpdateProductsWithInactiveCategories(id);
            int cnt = 0;
            if(productList != null)
            {
                foreach (var item in productList)
                {
                    if (prodSvc.UpdateProduct(item))
                        cnt += 1;
                }
            }
            return cnt;
        }
        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            return service;
        }

        private ProductService CreateProductService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ProductService(userId);
            return service;
        }
    }
}