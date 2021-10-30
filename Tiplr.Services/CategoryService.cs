using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateProductCategory(CategoryCreate model)
        {
            var entity = new ProductCategory()
            {
                CategoryName = model.CategoryName,
                Active = true
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.ProductCategories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CategoryListItem> GetCategories()
        {
            using( var ctx = new ApplicationDbContext())
            {
                var query = ctx.ProductCategories.Where(e => e.CategoryId > 0).OrderBy(e => e.Active).ThenBy(e => e.CategoryName).
                    Select(e => new CategoryListItem
                    {
                        CategoryId = e.CategoryId,
                        CategoryName = e.CategoryName,
                        Active = e.Active
                    });
                return query.ToArray();
            }
        }

        public CategoryDetail GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.ProductCategories.Single(e => e.CategoryId == id);
                return new CategoryDetail
                {
                    CategoryId = entity.CategoryId,
                    CategoryName = entity.CategoryName,
                    Active = entity.Active
                };
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.ProductCategories.Single
                    (e => e.CategoryId == model.CategoryId);

                entity.CategoryName = model.CategoryName;
                entity.Active = model.Active;

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
