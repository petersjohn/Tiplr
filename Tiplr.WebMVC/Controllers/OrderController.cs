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
    public class OrderController : Controller
    {
        [Authorize]
        // GET: Order
        public ActionResult Index()
        {
            var svc = CreateOrderService();
            var model = svc.GetOrders();
            return View(model);
        }

        //Get Create

        public ActionResult Create()
        {
            var orderSvc = CreateOrderService();
            var InvItemSvc = CreateInvItemService();
            int currentInv = InvItemSvc.GetCurrentInvId();
            var viewModel = orderSvc.CreateOrderView();
            viewModel.InventoryId = currentInv;
            return View(viewModel);
        }

        //Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            var svc = CreateOrderService();
            if (svc.CreateOrder(model))
            {
                TempData["SaveResult"] = "Your Order was created!";

                return RedirectToAction("Index");//reset this to return the user to the order item index.
            };
            ModelState.AddModelError("", "Order could not be created. Either no inventory has been started or there is an existing order for this inventory period.");
            return View(model);

        }

        //Edit GetThe Order
        public ActionResult Edit(int id)
        {
            var ordSvc = CreateOrderService();
            var ordStatSvc = CreateStatusSvc();
            var orderDetail = ordSvc.GetOrderById(id);
            var status = ordStatSvc.GetStatus();
            var model = new OrderEdit
            {
                OrderId = orderDetail.OrderId,
                OrderStatusId = orderDetail.OrderStatusId,
                Statuses = status.Select(s => new SelectListItem
                {
                    Text = s.OrderStatusMeaning,
                    Value = s.OrderStatusId.ToString()
                }),
                LastUpdateUserId = User.Identity.GetUserId()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.OrderId != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }
            var svc = CreateOrderService();
            if (svc.UpdateOrderStatus(model))
            {
                TempData["SaveResult"] = "The order was updated.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "The supplied order could not be updated.");
            return View(model);

        }
        public ActionResult Delete(int? id)
        {
            var svc = CreateOrderService();
            OrderDetail order = svc.GetOrderById((int)id);
            if (order == null)
                return View(HttpNotFound());
            return View(order);

        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id)
        {
            
            var ordSvc = CreateOrderService();
            int itemsDeleted = DeleteRelatedOrderItems(id);

            if (ordSvc.DeleteOrder(id))
            {
                TempData["SaveResult"] = $"Order ID {id} was deleted along with {itemsDeleted} ordered products related to this order";
                return RedirectToAction("Index");
            }
            else //return the user to the orde they are trying to delete.
            {
                var model = ordSvc.GetOrderById(id);
                ModelState.AddModelError("", $"Order ID {id} could not be deleted.");
                return View(model);
            }
        }


        //helper methods


        private int DeleteRelatedOrderItems(int orderId)
        {
            var ordItemSvc = CreateOrderItemService();
            var orderItems = ordItemSvc.GetOrderListItemsByOrderId(orderId);
            int OrderItemDeleteCount = 0;
            foreach(var item in orderItems)
            {
                if (ordItemSvc.DeleteOrderItem(item.OrderItemId))
                {
                    OrderItemDeleteCount += 1;
                };
            };
            return OrderItemDeleteCount;
        }
        private OrderService CreateOrderService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderService(userId);
            return service;
        }

        private OrderItemService CreateOrderItemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderItemService(userId);
            return service;
        }

        private InventoryItemService CreateInvItemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new InventoryItemService(userId);
            return service;
        }

        private OrderStatusService CreateStatusSvc()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderStatusService(userId);
            return service;
        }

    }


}