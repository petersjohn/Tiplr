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
            if (FinalizedOpenCheck() > 0) //do not allow a new inventory to be created if there are open inventories
            {
                return false;
            }
            var entity = new Inventory()
            {
                InventoryDate = model.InventoryDate,
                CreatedByUser = _userId.ToString(),
                Finalized = false,
                LastModifiedDtTm = DateTimeOffset.Now,
                LastModUser = _userId.ToString()
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Inventories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public InventoryCreate GetInvCreateView()
        {
            var viewModel = new InventoryCreate();
            viewModel.InventoryDate = DateTimeOffset.Now;
            return viewModel;
        }

        public InventoryDetail GetInventoryById(int invId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.InventoryId == invId);
                return new InventoryDetail
                {
                    InventoryId = entity.InventoryId,
                    InventoryDate = entity.InventoryDate,
                    Finalized = entity.Finalized,
                    LastModUser = entity.LastModUser,
                    LastModifiedUser = entity.LastModBy,
                    CreatedByUser = entity.CreatedByUser,
                    CreateUser = entity.CreatedBy,
                    TotalOnHandValue = entity.TotalOnHandValue,
                    LastModifiedDtTm = entity.LastModifiedDtTm

                };
            }
        }
        public IEnumerable<InventoryList> GetInventories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Inventories.Where(
                    e => e.InventoryId > 0).OrderByDescending(e => e.InventoryDate)
                    .Select(e =>
                            new InventoryList
                            {
                                InventoryId = e.InventoryId,
                                Finalized = e.Finalized,
                                TotalOnHandValue = e.TotalOnHandValue,
                                InventoryDate = e.InventoryDate
                            });
                return query.ToArray();
            }
        }

        public bool UpdateInventory(InventoryUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.InventoryId == model.InventoryId);
                entity.Finalized = model.Finalized;
                entity.LastModifiedDtTm = DateTimeOffset.Now;
                entity.LastModUser = model.LastModifiedBy;
                entity.TotalOnHandValue = GetOnHandValue(model.InventoryId);


                return ctx.SaveChanges() == 1;
            }

        }

        public bool DeleteInventory(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Inventories.Single(e => e.InventoryId == id);
                if (entity.Finalized == true)
                {
                    return false;
                }
                else
                {
                    ctx.Inventories.Remove(entity);
                }
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
        public decimal GetOnHandValue(int id)
        {
            decimal totCnt = 0.00m;  //initialize
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.InventoryItems.Where(e => e.InventoryId == id)
                    .Select(e => new InvItemDetail
                    {
                        InventoryId = e.InventoryId,
                        OnHandCount = e.OnHandCount,
                        ProductId = e.ProductId,
                        Product = e.Product,
                        UpdtUser = e.LastModifiedById
                    });
                var invItemList = query.ToArray();
                foreach (var item in invItemList)
                {
                    totCnt += item.Product.UnitPrice * item.OnHandCount;
                }
                return totCnt;
            }
        }
    }
}


