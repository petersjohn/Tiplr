using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class InventoryItemService
    {
        private readonly Guid _userId;

        public InventoryItemService(Guid userId)
        {
            _userId = userId;
        }

        public bool InventoryCountCreate(InventoryItemCreate model)
        {
            var entity = new InventoryItem()
            {
                InventoryId = model.InventoryId,
                ProductId = model.ProductId,
                OnHandCount = model.OnHandCount,
                LastModifiedDtTm = DateTimeOffset.Now,
                Id = model.Id
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.InventoryItems.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool CreateCountList(IEnumerable<ProductListItem> Products)
        {
            var model = new InventoryItemCreate();
            model.InventoryId = GetCurrentInvId();
            //return the result model, or maybe create an item result model so we can return a view for items that failed to create, yay NOTES!!
            int saveCnt = 0;
            foreach (var item in Products)
            {
                model.OnHandCount = 0;
                model.ProductId = item.ProductId;
                model.Id = _userId.ToString();
                model.LastModifiedDtTm = DateTimeOffset.Now;
                if (InventoryCountCreate(model)) saveCnt += 1;
            }
            if (saveCnt > 0)
                return true;
            return false;
        }
        public IEnumerable<InventoryCountItem> GetOnHandInventory(int inventoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.InventoryItems.Where(e => e.InventoryId == inventoryId).OrderBy(e => e.Product.ProductCategory).ThenBy(e => e.Product.ProductName)
                    .Select(e => new InventoryCountItem
                    {
                        InventoryItemId = e.InventoryItemId,
                        InventoryId = e.InventoryId,
                        ProductId = e.ProductId,
                        OnHandCount = e.OnHandCount
                    });
                return query.ToArray();
            }
        }

        public IEnumerable<InventoryCountItem> GetCountsByProductCategory(int inventoryId, int productCatId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.InventoryItems.Where(q => q.InventoryId == inventoryId && q.Product.CategoryId == productCatId).OrderBy(q => q.Product.ProductName)
                    .Select(q => new InventoryCountItem
                    {
                        InventoryItemId = q.InventoryItemId,
                        InventoryId = q.InventoryId,
                        ProductId = q.ProductId,
                        OnHandCount = q.OnHandCount
                    });
                return query.ToArray();

            }
        }

        public InvItemDetail GetInvItemDetail(int invItemId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.InventoryItems.Single(e => e.InventoryItemId == invItemId);
                return new InvItemDetail
                {
                    InventoryItemId = entity.InventoryItemId,
                    InventoryId = entity.InventoryId,
                    ProductId = entity.ProductId,
                    OnHandCount = entity.OnHandCount,
                    UpdtUser = entity.Id //user string guid
                };
            }
        }

        public bool UpdateInvItem(InvItemEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.InventoryItems.Single(e => e.InventoryItemId == model.InventoryItemId);
                entity.OnHandCount = model.OnHandCount;
                entity.LastModifiedDtTm = DateTimeOffset.Now;
                entity.Id = model.Id;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteInvItem(int invItemId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.InventoryItems.Single(e => e.InventoryItemId == invItemId);
                ctx.InventoryItems.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        //helper
        private ProductService CreateProductService()
        {
            var userId = _userId;
            var service = new ProductService(userId);
            return service;
        }
        private int GetCurrentInvId()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.Finalized == false);
                return entity.InventoryId;
            }
        }
    }
}
