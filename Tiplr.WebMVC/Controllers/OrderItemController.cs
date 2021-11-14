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
    public class OrderItemController : Controller
    {
        [Authorize]
        // GET: OrderItem
        public ActionResult Index(int? id)//the was a null before
        {
            var orderItemSvc = CreateOrderItemService();
            var orderSvc = CreateOrderService();
            if (id == null)
            {
              id = orderSvc.GetCurrentOrderId();
            }

            var model = orderItemSvc.GetOrderListItemsByOrderId((int)id);
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
            var svc = CreateOrderItemService();
            if (!ModelState.IsValid) return View(model);
            if (svc.CreateOrderItem(model))
            {
                svc.UpdateOrderedInd(model.InventoryItemId,true);
                return RedirectToAction("Index","InventoryItem"); 
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
                OrderItemTotalPrice = detail.OrderItemTotalPrice,
                ProductId = detail.ProductId,
                Product = detail.Product

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
            //model.OrderItemTotalPrice = model.Product.CasePackPrice * model.OrderAmt;
            model.OrderItemTotalPrice = GetOrderItemTotal(model.ProductId, model.OrderAmt);
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
            var model = svc.GetOrderItemById(id);
            if (svc.DeleteOrderItem(id))
            {
                svc.UpdateOrderedInd(model.InventoryItemId, false);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "The order item could not be deleted.Either the id was invalid or the order cost could not be adjusted.");
            return View(model);
        }


        //helper methods

        private decimal GetOrderItemTotal(int productId, int orderAmt)
        {
            var prdSvc = CreateProductService();
            var product = prdSvc.GetProductById(productId);
            decimal orderPrice = product.CasePackPrice * orderAmt;
            return orderPrice;
        }
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

        private ProductService CreateProductService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ProductService(userId);
            return service;
        }
        private OrderStatusService CreateStatusSvc()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderStatusService(userId);
            return service;
        }
        private string GetOrderStatusByOrderItemId(int orderItemId)
        {
            var ordItemSvc = CreateOrderItemService();
            //var statSvc = CreateStatusSvc();
            var ordItem = ordItemSvc.GetOrderItemById(orderItemId);
            string status = ordItem.Order.OrderStatus.OrderStatusMeaning;
            return status;
            //var status = statSvc.GetStatusById(ordItem.Order.)
        }
    }
}