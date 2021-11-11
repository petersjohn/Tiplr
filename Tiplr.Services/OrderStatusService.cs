using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;
using Tiplr.Models;

namespace Tiplr.Services
{
    public class OrderStatusService
    {
        private readonly Guid _userId;

        public OrderStatusService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<OrderStatusDetail> GetStatus()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.OrderStatuses.Where(e=> e.Active == true).OrderBy(e => e.OrderStatusMeaning)
                    .Select(e => new OrderStatusDetail
                    {
                        OrderStatusId = e.OrderStatusId,
                        OrderStatusMeaning = e.OrderStatusMeaning
                    });
                return query.ToArray();
            }
        }

        public OrderStatusDetail GetStatusById(int? id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.OrderStatuses.Single(e => e.OrderStatusId == id);
                var detail = new OrderStatusDetail();
                detail.OrderStatusId = entity.OrderStatusId;
                detail.OrderStatusMeaning = entity.OrderStatusMeaning;
                return detail;
            }
            
        }

    }
}
