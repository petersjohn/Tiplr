using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool CreateOrderItem(OrderItemCreate model)
        {
            var entity = new OrderItem()
            {
                ProductId = model.ProductId,
                InventoryItemId = model.InventoryItemId,
                OrderId = model.OrderId,
                OrderAmt = model.OrderAmt,
                AmtReceived = model.AmtReceived
            };
            using ( var ctx = new ApplicationDbContext())
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
                                 OrderId = e.OrderId,
                                 ProductId = e.ProductId,
                                 OrderAmt = e.OrderAmt,
                                 AmtReceived = e.AmtReceived,
                                 InventoryItemId = e.InventoryItemId
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
                            select new OrderItemListItem
                            {
                                OrderItemId = ordItem.OrderItemId,
                                OrderId = ordItem.OrderId,
                                ProductId = ordItem.ProductId,
                                OrderAmt = ordItem.OrderAmt,
                                AmtReceived = ordItem.AmtReceived,
                                InventoryItemId = ordItem.InventoryItemId
                            };
                return query.ToArray();

            }
        }


    }
}
