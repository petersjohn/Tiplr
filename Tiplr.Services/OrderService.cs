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


        public bool CreateOrder(OrderCreate model)
        {
            var entity = new Order()
            {
                InventoryId = model.InventoryId,
                OrderStatusId = model.OrderStatusId,
                OrderCost = model.OrderCost,
                OrderDate = DateTimeOffset.Now,
                UserId = model.LastUpdateUserId
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Orders.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public OrderDetail GetOrderDetail(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var order = ctx.Orders.Single(o => o.OrderId == orderId);
                return new OrderDetail
                {
                    OrderId = order.OrderId,
                    InventoryId = order.InventoryId,
                    OrderCost = order.OrderCost,
                    UserId = order.UserId
                };

            }
        }
        public IEnumerable<OrderListItem> GetOrders()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Orders.Where(
                    e => e.OrderId > 0).OrderByDescending(e => e.OrderDate)
                    .Take(10).Select(
                    e => new OrderListItem
                    {
                        OrderId = e.OrderId,
                        InventoryId = e.InventoryId,
                        OrderStatusId = e.OrderStatusId,
                        OrderDate = e.OrderDate
                    });
                return query.ToArray();
            }
        }

        //Update
        public bool FinalizeOrder(OrderFinalize model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.OrderId == model.OrderId);
                entity.OrderStatusId = model.OrderStatusId;
                entity.UserId = model.LastUpdateUserId;
                

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
    }
}
