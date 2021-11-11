﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class OrderItemService
    {
        private readonly Guid _userId;

        public OrderItemService(Guid userId)
        {
            _userId = userId;
        }
        public OrderItemCreate GetOrderItemCreateView(int invItemId)//this will only get called from inv item page
        {
            var ctx = new ApplicationDbContext();
            var orderSvc = new OrderService(_userId);
            var itemEntity = ctx.InventoryItems.Single(ie => ie.InventoryItemId == invItemId);
            var returnModel = new OrderItemCreate
            {
                InventoryItemId = invItemId,
                OrderId = orderSvc.getCurrentOrderId(),
                ProductId = itemEntity.ProductId
            };

            return returnModel;
        }
        public bool CreateOrderItem(OrderItemCreate model)
        {
            var entity = new OrderItem()
            {
                ProductId = model.ProductId,
                InventoryItemId = model.InventoryItemId,
                OrderId = model.OrderId,
                OrderAmt = model.OrderAmt,
                AmtReceived = model.AmtReceived,
                OrderItemTotalPrice = model.Product.CasePackPrice * model.OrderAmt

            };
            using (var ctx = new ApplicationDbContext())
            {

                ctx.OrderItems.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<OrderItemListItem> GetOrderListItemsByOrderId(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.OrderItems.Where(
                    e => e.OrderId == orderId).OrderBy(e => e.Product.ProductCategory.CategoryName)
                    .ThenBy(e => e.Product.ProductName).Select(e =>
                             new OrderItemListItem
                             {
                                 OrderItemId = e.OrderItemId,
                                 OrderId = (int)e.OrderId,
                                 ProductId = e.ProductId,
                                 OrderAmt = e.OrderAmt,
                                 AmtReceived = e.AmtReceived,
                                 InventoryItemId = e.InventoryItemId,
                                 Product = e.Product,
                                 OrderItemCost = e.OrderItemTotalPrice
                             });
                return query.ToArray();
            }
        }

        public IEnumerable<OrderItemListItem> GetOrderListItemsByInvId(int inventoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = from invItem in ctx.InventoryItems
                            join ordItem in ctx.OrderItems on invItem.InventoryItemId
                                equals ordItem.InventoryItemId
                            where invItem.InventoryId == inventoryId
                            orderby ordItem.Product.ProductCategory.CategoryName, ordItem.Product.ProductName
                            select new OrderItemListItem
                            {
                                OrderItemId = ordItem.OrderItemId,
                                OrderId = (int)ordItem.OrderId,
                                ProductId = ordItem.ProductId,
                                OrderAmt = ordItem.OrderAmt,
                                AmtReceived = ordItem.AmtReceived,
                                InventoryItemId = ordItem.InventoryItemId
                            };
                return query.ToArray();

            }
        }

        public OrderItemDetail GetOrderItemById(int orderItemId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.OrderItems.Single(e => e.OrderItemId == orderItemId);
                return new OrderItemDetail
                {
                    OrderItemId = entity.OrderItemId,
                    ProductId = entity.ProductId,
                    InventoryItemId = entity.InventoryItemId,
                    OrderId = (int)entity.OrderId,
                    OrderAmt = entity.OrderAmt,
                    AmtReceived = entity.AmtReceived,
                    OrderItemTotalPrice = entity.OrderItemTotalPrice,
                    Order = entity.Order,
                    Product = entity.Product
                };
            }
        }

        public bool UpdateOrderItem(OrderItemEdit model)
        {

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.OrderItems.Single(e => e.OrderItemId == model.OrderItemId);
                var origCost = entity.OrderItemTotalPrice;
                entity.OrderAmt = model.OrderAmt;
                entity.AmtReceived = model.AmtReceived;

               
                if (model.AmtReceived > 0)
                {
                    entity.OrderItemTotalPrice = entity.Product.CasePackPrice * model.AmtReceived;
                }
                else
                {
                    entity.OrderItemTotalPrice = entity.Product.CasePackPrice * model.OrderAmt;
                }

                if(entity.Order.OrderStatus.OrderStatusMeaning != "Generated")
                {
                    AdjustOrderCost(entity.OrderItemTotalPrice, origCost, (int)entity.OrderId);
                }
                return ctx.SaveChanges() > 0;
            }
        }

        public bool DeleteOrderItem(int orderItemId)
        {
            decimal costAdjustment;
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.OrderItems.Single(e => e.OrderItemId == orderItemId);
                costAdjustment = entity.OrderItemTotalPrice * -1;
                if (AdjustOrderCost(costAdjustment, 0.00m, (int)entity.OrderId)) //only remove if the cost can be adjusted
                {
                    ctx.OrderItems.Remove(entity);
                    return ctx.SaveChanges() == 1;
                }
                return false;
            }

        }
        //helper method
        public bool AdjustOrderCost(decimal CostAdjustment, decimal origCost, int orderId)
        {
            decimal entityCost;
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.OrderId == orderId);
                entityCost = entity.OrderCost;
                entity.OrderCost = (entityCost - origCost) + CostAdjustment;
                return ctx.SaveChanges() == 1;
            }
        }

        public int GetOrderItemVolume(int productId,decimal onHand)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Products.Single(e => e.ProductId == productId);
                decimal parDiff = entity.Par - onHand;
                int orderAmt = (int)Math.Ceiling(parDiff / entity.UnitsPerPack);
                 return orderAmt;
            }
        }

        



    }  
}
