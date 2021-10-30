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
    public class InventoryController : Controller
    {
        [Authorize]
        // GET: Inventory
        public ActionResult Index()
        {
            var svc = CreateInvService();
            var model = svc.GetInventories();
            return View(model);
        }

        //GET Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var invSvc = CreateInvService();
            var invItemSvc = CreateInvItemService();
            var prdSvc = CreateProductService();
            if (invSvc.CreateInventory(model))
            {
                var prdList = prdSvc.GetActiveProducts();
                if (invItemSvc.CreateCountList(prdList))
                {
                    TempData["SaveResult"] = "New Inventory Created";
                    return RedirectToAction("Index");
                };
                TempData["SaveResult"] = "New Inventory Started, but not all Inventory Items were created. DO SOMETHING JOHN";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Inventory could not be started");
            return View(model);


        }
        //helper methods


        private InventoryService CreateInvService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new InventoryService(userId);
            return service;
        }

        private InventoryItemService CreateInvItemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new InventoryItemService(userId);
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