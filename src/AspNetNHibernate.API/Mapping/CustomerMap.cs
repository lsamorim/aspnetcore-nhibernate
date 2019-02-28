using AspNetNHibernate.API.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace AspNetNHibernate.API.Mapping
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.MemberSince);
            Map(x => x.HasGoldStatus);
            Map(x => x.Points);
            Map(x => x.CreditRating).CustomType<EnumStringType<CustomerCreditRating>>();
            Component(x => x.Address);
            HasMany(x => x.Orders);
        }
    }
}
