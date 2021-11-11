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
    public class ProductController : Controller
    {
        [Authorize]
        // GET: Product
        public ActionResult Index()
        {
            var svc = CreateProductService();
            var model = svc.GetActiveProducts();
            return View(model);
        }

        //GET: Create
        public ActionResult Create()
        {
            var svc = CreateProductService();
            var viewModel = svc.GetCreateView();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var svc = CreateProductService();
            if (svc.CreateProduct(model))
            {
                TempData["SaveResult"] = "Product successfully created.";
                return RedirectToAction("Index");
            };
            ModelState.AddModelError("", "Product could not be created.");
            return View(model);

        }

        public ActionResult Details(int id)
        {
            var svc = CreateProductService();
            var model = svc.GetProductById(id);
            return View(model);
        }

        //Get product
        public ActionResult Edit(int id)
        {
            var svc = CreateProductService();
            var catSvc = CreateCategoryService();
            var detail = svc.GetProductById(id);
            var cat = catSvc.GetCategories();
            var model = new ProductEdit
            {
                ProductId = detail.ProductId,
                CategoryId = detail.CategoryId,
                Categories = cat.Select(e => new SelectListItem
                {
                    Text = e.CategoryName,
                    Value = e.CategoryId.ToString()
                }),
                ProductName = detail.ProductName,
                ProductDescription = detail.ProductDescription,
                CountBy = detail.CountBy,
                OrderBy = detail.OrderBy,
                Par = detail.Par,
                UnitsPerPack = detail.UnitsPerPack,
                CasePackPrice = detail.CasePackPrice,
                Active = detail.Active,
                Category = detail.Category 
                
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEdit model)
        {
            var catSvc = CreateCategoryService();
            var cat = catSvc.GetCategories();
            model.Categories = cat.Select(e => new SelectListItem
            {
                Text = e.CategoryName,
                Value = e.CategoryId.ToString()
            });
            
            if (!ModelState.IsValid) return View(model);
            if(model.ProductId != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }
            var svc = CreateProductService();
            if (svc.UpdateProduct(model))
            {
                TempData["Save Result"] = "Product successfully updated";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Product could not be updated");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var svc = CreateProductService();
            var catSvc = CreateCategoryService();
            var detail = svc.GetProductById(id);
            var cat = catSvc.GetCategories();
            var model = new ProductEdit
            {
                ProductId = detail.ProductId,
                CategoryId = detail.CategoryId,
                ProductName = detail.ProductName,
                ProductDescription = detail.ProductDescription,
                CountBy = detail.CountBy,
                OrderBy = detail.OrderBy,
                Par = detail.Par,
                UnitsPerPack = detail.UnitsPerPack,
                CasePackPrice = detail.CasePackPrice,
                Active = false,
                Category = detail.Category
            };
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ProductEdit model)
        {
            var svc = CreateProductService();
            if(id == model.ProductId && ModelState.IsValid)
            {
                svc.UpdateProduct(model);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Product could not be updated");
            return View(model);
        }

        //Helper Methods
        private ProductService CreateProductService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ProductService(userId);
            return service;
        }
        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            return service;
        }

    }
}