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
    public class InventoryItemController : Controller
    {


        //GET Create
        public ActionResult Create() //this would only get called once you have created a product
        {
            var svc = CreateInvItemService();
            var model = svc.CreateInvItemView();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryItemCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var invItemSvc = CreateInvItemService();
            decimal invAdj = model.OnHandCount * GetUnitPrice(model.ProductId);
            if (invItemSvc.CreateInvItemCount(model))
            {
                TempData["SaveResult"] = "InventoryItem added to Count Sheet..";
                if (UpdateInventory(model.InventoryId, invAdj))
                {
                    TempData["SaveResult"] = "Inventory total updated";
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update inventory.");
                }

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to create the item and add it to the count sheet.");
            return View(model);
        }

        //GET All InvItems
        public ActionResult Index()
        {
            var svc = CreateInvItemService();
            int invId = svc.GetCurrentInvId();
            var viewModel = svc.GetOnHandInventory(invId);
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateInvItemService();
            var model = svc.GetInventoryItemById(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var svc = CreateInvItemService();
            var model = svc.GetInventoryItemById(id);
            var viewModel = new InvItemEdit
            {
                LastModBy = model.UpdtUser,
                InventoryItemId = model.InventoryItemId,
                InventoryId = model.InventoryId,
                OnHandCount = model.OnHandCount,
                ProductId = model.ProductId,
                Product = model.Product,
                LastModifiedDateTime = DateTimeOffset.Now,
                LastModifiedBy = model.LastUpdtBy,
                OrderedInd = model.OrderedInd
            };
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id, InvItemEdit model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.InventoryItemId != id)
            {
                ModelState.AddModelError("", "ID mismatch");
                return View(model);
            }
            decimal origCostOfItem = GetItemCost(id);
            decimal updtdCostOfItem = model.OnHandCount * GetUnitPrice(model.ProductId);
            decimal adjToInvCost = updtdCostOfItem - origCostOfItem;
            var invItemSvc = CreateInvItemService();

            if (invItemSvc.UpdateInvItem(model))
            {
                TempData["SaveResult"] = "Count list item update successful.";
                if (UpdateInventory(model.InventoryId, adjToInvCost))
                {
                    TempData["SaveResult"] = "Inventory update successful.";
                }
                else
                {
                    ModelState.AddModelError("", "WARNING: Inventory Cost failed to update.");
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Inventory count item update failed.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var invItemSvc = CreateInvItemService();
            var model = invItemSvc.GetInventoryItemById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ItemDelete(int id)
        {   
            
            var invItemSvc = CreateInvItemService();
            var model = invItemSvc.GetInventoryItemById(id);
            var adjAmt = GetItemCost(model.InventoryItemId) * -1;
            if (invItemSvc.DeleteInvItem(model.InventoryItemId))
            {
                TempData["SaveResult"] = "Count item update deleted.";

                if (UpdateInventory(model.InventoryId, adjAmt))
                {
                    TempData["SaveResult"] = "Inventory updated.";
                }
                else
                {
                    ModelState.AddModelError("", "Inventory update failed.");
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Deletion of count item failed.");
            return View(model);
        }

        //helper methods
        //**************************************************************************************

        private decimal GetItemCost(int id)
        {
            var svc = CreateInvItemService();
            var model = svc.GetInventoryItemById(id);
            decimal costOfItem = model.OnHandCount * model.Product.UnitPrice;
            return costOfItem;
        }

        private decimal GetUnitPrice(int productId)
        {
            var prdSvc = CreateProductService();
            var model = prdSvc.GetProductById(productId);
            return model.UnitPrice;
        }

        private bool UpdateInventory(int inventoryId, decimal adjAmt)
        {
            var invSvc = CreateInvService();
            var detail = invSvc.GetInventoryById(inventoryId);
            var updtModel = new InventoryUpdate
            {
                InventoryId = detail.InventoryId,
                LastModifiedBy = User.Identity.GetUserId(),
                Finalized = detail.Finalized,
                TotalOnHandValue = detail.TotalOnHandValue + adjAmt

            };
            if (invSvc.UpdateInventory(updtModel))
            {
                return true;
            }
            return false;
        }

        private InventoryItemService CreateInvItemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new InventoryItemService(userId);
            return service;
        }

        private InventoryService CreateInvService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new InventoryService(userId);
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