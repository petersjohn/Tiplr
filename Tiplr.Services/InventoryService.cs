using System;
using System.Collections.Generic;
using System.Linq;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class InventoryService
    {
        private readonly Guid _userId;

        public InventoryService(Guid userId)
        {
            _userId = userId;
        }
        public bool CreateInventory(InventoryCreate model)
        {
            if(FinalizedOpenCheck() > 0) //do not allow a new inventory to be created if there are open inventories
            {
                return false;
            }
            var entity = new Inventory()
            {
                InventoryDate = DateTimeOffset.Now,//model.InventoryDate
                CreatedByUser = _userId,
                Finalized = false,
                LastModifiedDtTm = DateTimeOffset.Now,
                UpdtUser = _userId
            };
           using (var ctx = new ApplicationDbContext())
            {
                ctx.Inventories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<InventoryList> GetInventories()
        {
            using( var ctx = new ApplicationDbContext())
            {
                var query = ctx.Inventories.Where(
                    e => e.InventoryId > 0).OrderByDescending(e => e.InventoryDate)
                    .Select(e =>
                            new InventoryList
                            {
                                InventoryId = e.InventoryId,
                                Finalized = e.Finalized,
                                InventoryDate = e.InventoryDate
                            });
                return query.ToArray();
            }
        }

        public bool UpdateInventory(InventoryFinalize model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.InventoryId == model.InventoryId);
                entity.Finalized = model.Finalized;
                entity.LastModifiedDtTm = DateTimeOffset.Now;
                entity.UpdtUser = _userId;

                return ctx.SaveChanges() == 1;
             }
           
        }

        public bool DeleteInventory(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.InventoryId == id);
                ctx.Inventories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        //helper method
        private int FinalizedOpenCheck()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var count = ctx.Inventories.Where(e => e.Finalized == false).Count();
                return count;
            }
        }
        



    }
}
