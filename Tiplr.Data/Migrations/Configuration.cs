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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            IList<OrderStatus> status = new List<OrderStatus>();

            status.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Accepted",
                Active = true
            });
            status.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Generated",
                Active = true
            });
            status.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Placed with Vendor",
                Active = true
            });
            status.Add(new OrderStatus()
            {
                OrderStatusMeaning = "Checked In",
                Active = true
            });

            foreach(var item in status)
            {
                context.OrderStatuses.AddOrUpdate(item);
                base.Seed(context);

            }

        }
    }
}
