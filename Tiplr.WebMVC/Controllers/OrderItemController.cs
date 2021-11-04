﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tiplr.Models;
using Tiplr.Services;

namespace Tiplr.WebMVC.Controllers
{
    public class OrderItemController : Controller
    {
        [Authorize]
        // GET: OrderItem
        public ActionResult Index()
        {
            var orderItemSvc = CreateOrderItemService();
            var orderSvc = CreateOrderService();
            int orderId = orderSvc.getCurrentOrderId();
            var model = orderItemSvc.GetOrderListItemsByOrderId(orderId);
            return View(model);
        }
        //Get Create
        public ActionResult Create(int invItemId)//this is only called from invItemList view
        {
            var orderItemSvc = CreateOrderItemService();
            var viewModel = orderItemSvc.GetOrderItemCreateView(invItemId);
            return View(viewModel);

        }
        //Create OrderItem
        [ActionName("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderItemCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var svc = CreateOrderItemService();
            if (svc.CreateOrderItem(model))
            {
                TempData["SaveResult"] = "Manual product entry added to current order!";
                return RedirectToAction("Index"); 
            };
            ModelState.AddModelError("", "Order item could not be created.");
            return View(model);
        }

        //Get {id}

        public ActionResult Detail(int id)
        {
            var svc = CreateOrderItemService();
            var model = svc.GetOrderItemById(id);
            return View(model);
        }

        //get orderinv item view
        public ActionResult Edit(int id)
        {
            var ordItemSvc = CreateOrderItemService();
            var detail = ordItemSvc.GetOrderItemById(id);
            var model = new OrderItemEdit
            {
                OrderAmt = detail.OrderAmt,
                AmtReceived = detail.AmtReceived,
                OrderItemId = detail.OrderItemId,
                OrderItemTotalPrice = detail.OrderItemTotalPrice //gonna need help here hide this
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id, OrderItemEdit model)
        {
            if (!ModelState.IsValid) return View(model);
            if(model.OrderItemId != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }
            var svc = CreateOrderItemService();
            //update the orderItemTotalPrice
            model.OrderItemTotalPrice = model.Product.CasePackPrice * model.OrderAmt;
            if (svc.UpdateOrderItem(model))
            {
                TempData["SaveResult"] = "Order item was successfully updated.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "FML, what is this??");
            return View(model);

        }

        public  ActionResult Delete(int id)
        {
            var svc = CreateOrderItemService();
            var model = svc.GetOrderItemById(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrderItem(int id)
        {
            var svc = CreateOrderItemService();
            if (svc.DeleteOrderItem(id))
            {
                TempData["SaveResult"] = "Order item was deleted.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "The order item could not be deleted.Either the id was invalid or the order cost could not be adjusted.");
            var model = svc.GetOrderItemById(id);
            return View(model);
        }


        //helper methods

        private OrderItemService CreateOrderItemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderItemService(userId);
            return service;
        }

        private OrderService CreateOrderService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderService(userId);
            return service;
        }
    }
}