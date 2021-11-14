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
            var errMsg = TempData["ErrorMessage"] as string;
            var svc = CreateOrderService();
            var model = svc.GetOrders();
            if (errMsg != null)
            {
                ViewBag.Message = "Cannot display order items for current order...because there is no current order.";
            }
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
                CreateOrderItemsFromOrderCreate(model.InventoryId);

                return RedirectToAction("Index", "OrderItem");//reset this to return the user to the order item index.
            };
            ModelState.AddModelError("", "Order could not be created. Either no inventory has been started or there is an existing order for this inventory period.");
            return View(model);

        }

        public ActionResult Details(int id)
        {
            var svc = CreateOrderService();
            var model = svc.GetOrderById(id);
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
                LastUpdateUserId = User.Identity.GetUserId(),
                OrderCost = orderDetail.OrderCost,
                OrderStatus = orderDetail.OrderStatus


            };
            return View(model);
        }
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderEdit model)
        {
            var ordStatSvc = CreateStatusSvc();
            var status = ordStatSvc.GetStatus();
            model.Statuses = status.Select(e => new SelectListItem
            {
                Text = e.OrderStatusMeaning,
                Value = e.OrderStatusId.ToString()
            });
            if (!ModelState.IsValid) return View(model);

            if (model.OrderId != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }
            var statModel = CheckOrderStatus((int)model.OrderStatusId);
            if(statModel.OrderStatusMeaning != "Generated")
            {
                model.OrderCost = GetOrderCost(model.OrderId);
            }
            var svc = CreateOrderService();
            if (svc.UpdateOrder(model))
            {
                TempData["SaveResult"] = "The order was updated.";
                //if(model.OrderStatus.OrderStatusMeaning == "Accepted")
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
                return View(HttpNotFound("The requested order does not exist. This is awkward."));
            return View(order);

        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id)
        {

            var ordSvc = CreateOrderService();


            if (ordSvc.DeleteOrder(id))
            {
                int itemsDeleted = DeleteRelatedOrderItems(id);
                TempData["SaveResult"] = $"Order ID {id} was deleted along with {itemsDeleted}  products related to this order";
                return RedirectToAction("Index");
            }
            else //return the user to the order they are trying to delete.
            {
                var model = ordSvc.GetOrderById(id);
                ViewBag.Message = "Orders with status of PLACED WITH VENDOR or CHECKED IN cannot be deleted...womp-womp.";
                return View(model);
            }
        }


        //helper methods
        private OrderStatusDetail CheckOrderStatus(int statusId)
        {
            var svc = CreateStatusSvc();
            var retModel = svc.GetStatusById(statusId);
            return retModel;
        }
        private int DeleteRelatedOrderItems(int orderId)
        {
            var ordItemSvc = CreateOrderItemService();
            var orderItems = ordItemSvc.GetOrderListItemsByOrderId(orderId);
            int OrderItemDeleteCount = 0;
            foreach (var item in orderItems)
            {
                if (ordItemSvc.DeleteOrderItem(item.OrderItemId))
                {
                    OrderItemDeleteCount += 1;
                };
            };
            return OrderItemDeleteCount;
        }

        private int CreateOrderItemsFromOrderCreate(int invId)
        {
            var ordSvc = CreateOrderService();
            var ordItemSvc = CreateOrderItemService();
            var prdSvc = CreateProductService();
            var itemsToOrder = GetInvItemsToOrder(invId);
            int orderId = ordSvc.GetCurrentOrderId();
            int cnt = 0;
            OrderItemCreate createOrderItem = new OrderItemCreate();
            foreach (var item in itemsToOrder)
            {
                createOrderItem.ProductId = item.ProductId;
                createOrderItem.InventoryItemId = item.InventoryItemId;
                createOrderItem.OrderId = orderId;
                createOrderItem.OrderAmt = ordItemSvc.GetOrderItemVolume(item.ProductId, item.OnHandCount);
                createOrderItem.AmtReceived = 0;
                createOrderItem.CasePackPrice = item.Product.CasePackPrice;
                if (ordItemSvc.CreateOrderItem(createOrderItem))
                {
                    cnt += 1;
                    ordItemSvc.UpdateOrderedInd(createOrderItem.InventoryItemId, true);
                }
                    
                
            }
            return cnt;
        }
        private OrderService CreateOrderService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new OrderService(userId);
            return service;
        }
        private List<InvItemDetail> GetInvItemsToOrder(int invId)
        {
            var invItemSvc = CreateInvItemService();
            var subPar = invItemSvc.GetSubParInvItems(invId);
            return subPar;
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

        private ProductService CreateProductService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ProductService(userId);
            return service;
        }

        public decimal GetOrderCost(int orderId)
        {
            var orderItemSvc = CreateOrderItemService();
            var orderSvc = CreateOrderService();
            var orderItems = orderItemSvc.GetOrderListItemsByOrderId(orderId);
            var model = new OrderEdit();
            decimal orderCost = 0;
            foreach (var item in orderItems)
            {
                orderCost += item.OrderItemCost;
            }
            return orderCost;
        }


    }


}