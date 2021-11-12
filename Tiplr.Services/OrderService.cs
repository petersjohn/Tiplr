using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class OrderService
    {
        private readonly Guid _userId;

        public OrderService(Guid userId)
        {
            _userId = userId;
        }

        public OrderCreate CreateOrderView()
        {
            var model = new OrderCreate();
            var ctx = new ApplicationDbContext();
            model.OrderStatusId = ctx.OrderStatuses.Single(e => e.OrderStatusMeaning == "Generated").OrderStatusId;
            model.OrderCost = 0.00m;
            model.OrderDate = DateTimeOffset.Now;
            model.LastUpdateUserId = _userId.ToString();
            return model;
        }
        public bool CreateOrder(OrderCreate model)
        {
            if (ValidateNoOrderExists(model.InventoryId) && ValidateInvExists(model.InventoryId))//don't create an order if one already exists OR if an inventory has not been created
            {
                var entity = new Order()
                {
                    InventoryId = model.InventoryId,
                    OrderStatusId = model.OrderStatusId,
                    OrderCost = model.OrderCost,
                    OrderDate = DateTimeOffset.Now,
                    LastModifiedById = model.LastUpdateUserId
                };
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Orders.Add(entity);
                    return ctx.SaveChanges() == 1;
                }
            }
            return false;
        }

        public OrderDetail GetOrderById(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var order = ctx.Orders.Single(o => o.OrderId == orderId);
                return new OrderDetail
                {
                    OrderId = order.OrderId,
                    InventoryId = order.InventoryId,
                    OrderCost = order.OrderCost,
                    UserId = order.LastModifiedById,
                    OrderDate = order.OrderDate,
                    OrderStatusId = (int)order.OrderStatusId,
                    OrderStatus = order.OrderStatus,
                    ApplicationUser = order.LastModBy,
                    Inventory = order.Inventory,
                    
                };

            }
        }
        public IEnumerable<OrderListItem> GetOrders()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Orders.Where(
                    e => e.OrderId > 0).OrderByDescending(e => e.OrderDate)
                    .Select(
                    e => new OrderListItem
                    {
                        OrderId = e.OrderId,
                        InventoryId = e.InventoryId,
                        OrderStatusId = (int)e.OrderStatusId,
                        OrderDate = e.OrderDate,
                        OrderStatus = e.OrderStatus

                    });
                return query.ToArray();
            }
        }

        //Update
        public bool UpdateOrder(OrderEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.OrderId == model.OrderId);
                entity.OrderStatusId = model.OrderStatusId;
                entity.LastModifiedById = model.LastUpdateUserId;
                entity.OrderCost = model.OrderCost;
                return ctx.SaveChanges() == 1;
            }
        }
      
        public bool DeleteOrder(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.OrderId == orderId);
                ctx.Orders.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        //helper method
        public bool ValidateNoOrderExists(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var orderCount = ctx.Orders.Count(e => e.InventoryId == id);
                if (orderCount > 0)
                    return false;
                return true;
            }
        }

        public bool ValidateInvExists(int id)
        {
            var ctx = new ApplicationDbContext();
            if (ctx.Inventories.Find(id) == null)
            {
                return false;
            }
            return true;
        }

        public int GetCurrentOrderId()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = (from i in ctx.Inventories
                             join o in ctx.Orders on i.InventoryId equals o.InventoryId
                             where i.Finalized == false
                             select new { o.OrderId }).FirstOrDefault();
                if (query == null)
                    return 0;
                return query.OrderId;

            }
        }
    }
}
