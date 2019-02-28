using AspNetNHibernate.API.Entities;
using FluentNHibernate.Mapping;

namespace AspNetNHibernate.API.Mapping
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Id(x => x.Id);
            Map(x => x.OrderedAt);
            Map(x => x.Shipped);
            Component(x => x.ShipTo);

            References(x => x.Customer)
                .Column("CustomerId");
        }
    }
}
