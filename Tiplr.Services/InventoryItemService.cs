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

        public InventoryItemCreate CreateInvItemView(int productId)//this is a one off item if you add a product mid inventory cycle.
        {
            var model = new InventoryItemCreate
            {
                ProductId = productId,
                InventoryId = GetCurrentInvId(),
                OnHandCount = 0
            };
            return model;

        }
        
        public bool CreateInvItemCount(InventoryItemCreate model)
        {
            var entity = new InventoryItem()
            {
                InventoryId = model.InventoryId,
                ProductId = model.ProductId,
                OnHandCount = model.OnHandCount,
                LastModifiedDtTm = DateTimeOffset.Now,
                LastModifiedById = _userId.ToString()
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.InventoryItems.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool CreateCountList(IEnumerable<ProductListItem> Products)
        {
            var model = new InventoryItemCreate
            {
                InventoryId = GetCurrentInvId()
            };
            //return the result model, or maybe create an item result model so we can return a view for items that failed to create, yay NOTES!!
            int saveCnt = 0;
            foreach (var item in Products)
            {
                model.OnHandCount = 0;
                model.ProductId = item.ProductId;
                model.LastModUser = _userId.ToString();
                model.LastModifiedDtTm = DateTimeOffset.Now;
                if (CreateInvItemCount(model)) saveCnt += 1;
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

        public InvItemDetail GetInventoryItemById(int invItemId)
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
                    UpdtUser = entity.LastModifiedById //user string guid
                };
            }
        }

        public List<InvItemDetail> GetSubParInvItems(int inventoryId)
        {
            var invItems = GetOnHandInventory(inventoryId);
            List<InvItemDetail> orderInv = new List<InvItemDetail>();
            foreach(var item in invItems)
            {
                if(item.Product.Par < item.OnHandCount)
                {
                    orderInv.Add(new InvItemDetail
                    {
                        InventoryId = item.InventoryId,
                        OnHandCount = item.OnHandCount,
                        ProductId = item.ProductId,
                        UpdtUser = _userId.ToString(),
                        InventoryItemId = item.InventoryItemId
                    });
                }
            }
            return orderInv;
        }

        public bool UpdateInvItem(InvItemEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.InventoryItems.Single(e => e.InventoryItemId == model.InventoryItemId);
                entity.OnHandCount = model.OnHandCount;
                entity.LastModifiedDtTm = DateTimeOffset.Now;
                entity.LastModifiedById = model.LastModBy;

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
        
        public int GetItemInvRowCount(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var count = ctx.InventoryItems.Count(e => e.InventoryId == id);
                return count;
            }
        }

        public int GetCurrentInvId()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.Finalized == false);
                return entity.InventoryId;
            }
        }
       
    }
}
