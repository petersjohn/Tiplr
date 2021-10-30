using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class ProductService
    {
        private readonly Guid _userId;

        public ProductService(Guid userId)
        {
            _userId = userId;
        }

        public ProductCreate GetCreateView()
        {
            var ctx = new ApplicationDbContext();
            var viewModel = new ProductCreate();
            viewModel.ProductCategories = ctx.ProductCategories.Select(category => new SelectListItem
            {
                Text = category.CategoryName,
                Value = category.CategoryId.ToString()
            });
            return viewModel;
        }

        public bool CreateProduct(ProductCreate model)
        {
            var entity = new Product()
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                ProductCategory = model.ProductCategory,
                CountBy = model.CountBy,
                OrderBy = model.OrderBy,
                CasePackPrice = model.CasePackPrice,
                UnitsPerPack = model.UnitsPerPack,
                UnitPrice = Math.Round((model.CasePackPrice / model.UnitsPerPack), 2, MidpointRounding.AwayFromZero),
                Par = model.Par,
                Active = true,
                CreatedDtTm = DateTime.Now,
                LastModifiedDtTm = DateTime.Now,
                InactiveDtTm = null,
                Id = _userId.ToString()
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Products.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ProductListItem> GetActiveProducts()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Products
                    .Where(e => e.Active == true).OrderBy(e => e.CategoryId).ThenBy(e => e.ProductName)
                    .Select(e =>
                               new ProductListItem
                               {
                                   ProductId = e.ProductId,
                                   ProductName = e.ProductName,
                                   ProductDescription = e.ProductDescription,
                                   CategoryId = e.CategoryId,
                                   UnitsPerPack = e.UnitsPerPack,
                                   UnitPrice = e.UnitPrice,
                                   Par = e.Par,
                                   Active = e.Active
                               });
                return query.ToArray();
            }
        }

        public ProductDetail GetProductById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Products.Single(e => e.ProductId == id);
                return new ProductDetail
                {
                    ProductId = entity.ProductId,
                    ProductName = entity.ProductName,
                    ProductDescription = entity.ProductDescription,
                    CategoryId = entity.CategoryId,
                    CountBy = entity.CountBy,
                    OrderBy = entity.OrderBy,
                    UnitPrice = entity.UnitPrice,
                    UnitsPerPack = entity.UnitsPerPack,
                    CasePackPrice = entity.CasePackPrice,
                    Par = entity.Par,
                    LastModifiedDtTm = entity.LastModifiedDtTm,
                    UserId = entity.ApplicationUser.Id,
                    Active = entity.Active                   
                };
            }

        }
        public IEnumerable<ProductListItem> GetProductByName(string name)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Products
                    .Where(e => e.ProductName.Contains(name)).Select(e =>
                      new ProductListItem
                      {
                          ProductId = e.ProductId,
                          ProductName = e.ProductName,
                          ProductDescription = e.ProductDescription,
                          CategoryId = e.CategoryId,
                          UnitsPerPack = e.UnitsPerPack,
                          UnitPrice = e.UnitPrice,
                          Par = e.Par,
                          Active = e.Active
                      });
                return query.ToArray();
            }
        }

        public bool UpdateProduct(ProductEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Products.Single
                    (e => e.ProductId == model.ProductId);
                entity.ProductName = model.ProductName;
                entity.ProductDescription = model.ProductDescription;
                entity.CategoryId = model.CategoryId;
                entity.OrderBy = model.OrderBy;
                entity.CountBy = model.CountBy;
                entity.CasePackPrice = model.CasePackPrice;
                entity.UnitsPerPack = model.UnitsPerPack;
                entity.UnitPrice = Math.Round((model.CasePackPrice / model.UnitsPerPack), 2, MidpointRounding.AwayFromZero);
                entity.Par = model.Par;
                entity.LastModifiedDtTm = DateTimeOffset.Now;
                if (model.Active == false && entity.Active == true)
                {
                    entity.InactiveDtTm = DateTimeOffset.Now;
                }
                else
                {
                    entity.InactiveDtTm = null;
                }
                entity.Active = model.Active;
                entity.LastModifiedDtTm = DateTimeOffset.Now;

                return ctx.SaveChanges() == 1;

            }
        }

    }
}
