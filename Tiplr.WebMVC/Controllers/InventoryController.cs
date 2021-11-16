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
            var svc = CreateInvService();
            var viewModel = svc.GetInvCreateView();
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var invSvc = CreateInvService();
            var productSvc = CreateProductService();
            var invItemSvc = CreateInvItemService();
            if (invSvc.CreateInventory(model))
            {
                TempData["SaveResult"] = "Inventory Started, building new count sheet..";
                var productsToInventory = productSvc.GetActiveProducts();
                if (invItemSvc.CreateCountList(productsToInventory))
                {
                    TempData["SaveResult"] = "Count sheet is complete, get to counting!";
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", "InventoryItem",new { id = 0 });
                }
            }
            ModelState.AddModelError("", "Could not create inventory. There is likely an active, unfinalized inventory.");
            return View(model);
        }


        public ActionResult Details(int id)
        {
            var svc = CreateInvService();
            var model = svc.GetInventoryById(id);

            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var invSvc = CreateInvService();
            var invDetail = invSvc.GetInventoryById(id);
            var invValue = invSvc.GetOnHandValue(id);
            var model = new InventoryUpdate()
            {
                InventoryId = invDetail.InventoryId,
                TotalOnHandValue = invValue,
                Finalized = true,
                LastModifiedBy = User.Identity.GetUserId()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InventoryUpdate model, int id)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.InventoryId != id)
            {
                ModelState.AddModelError("", "ID mismatch");
                return View(model);
            }
            var invSvc = CreateInvService();
            if (invSvc.UpdateInventory(model))
            {
                TempData["SaveResult"] = "Inventory has been saved.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Inventory Could not be saved");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var invSvc = CreateInvService();
            var model = invSvc.GetInventoryById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteInventory(int id)
        {
            var invSvc = CreateInvService();
            var itemSvc = CreateInvItemService();
            var ordSvc = CreateOrderService();
            int itemCnt = itemSvc.GetItemInvRowCount(id);

            if (ordSvc.ValidateNoOrderExists(id))
            {
                if (RemoveInvForDeletedInventory(id) == itemCnt)
                {
                    if (invSvc.DeleteInventory(id))
                    {
                        TempData["SaveResult"] = "Inventory and all associated counts successfully deleted.";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "The selected inventory could not be deleted.");
                    return View();
                }
                ModelState.AddModelError("", "Not all the associated inventory items could be deleted. Delete was unsuccessful.");
                return RedirectToAction("Index");
            }
            //var model = invSvc.GetInventoryById(id);
            var model = invSvc.GetInventoryById(id);
            ViewBag.Message = "NO DELETE FOR YOU: There are orders associate to this inventory, please review, then delete the orders before deleting the inventory.";
            return View(model);
           
        }
            

        //***********************************Helper Methods*****************************************//
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
        private OrderService CreateOrderService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderService(userId);
            return service;
        }
        private int RemoveInvForDeletedInventory(int id)
        {
            var itemSvc = CreateInvItemService();
            var invItemList = itemSvc.GetOnHandInventory(id);
            int deleteCnt = 0;
            foreach(var inv in invItemList)
            {
                if (itemSvc.DeleteInvItem(inv.InventoryItemId))
                {
                    deleteCnt += 1;
                }
            }
            return deleteCnt;

        }
        //private bool CheckForOrder(int invId)
    }
}


