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
                var query = ctx.OrderStatuses.Where(e=> e.Active == true)
                    .Select(e => new OrderStatusDetail
                    {
                        OrderStatusId = e.OrderStatusId,
                        OrderStatusMeaning = e.OrderStatusMeaning
                    });
                return query.ToArray();
            }
        }

    }
}
