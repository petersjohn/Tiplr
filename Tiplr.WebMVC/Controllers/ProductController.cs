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
            var detail = svc.GetProductById(id);
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
                Active = detail.Active
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEdit model, int id)
        {
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