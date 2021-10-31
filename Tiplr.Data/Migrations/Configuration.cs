namespace Tiplr.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Tiplr.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Tiplr.Data.ApplicationDbContext context)
        {
            List<OrderStatus> statusList = new List<OrderStatus>();

            statusList.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Accepted",
                Active = true
            });

            statusList.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Generated",
                Active = true
            });

            statusList.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Received",
                Active = true
            });

            statusList.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Checked In",
                Active = true
            });

            statusList.ForEach(status => context.OrderStatuses
            .AddOrUpdate(s => new { s.OrderStatusMeaning, s.Active }, status));
            context.SaveChanges();
        }
    }
}
