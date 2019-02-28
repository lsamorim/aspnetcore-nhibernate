using AspNetNHibernate.API.Entities;
using FluentNHibernate.Mapping;

namespace AspNetNHibernate.API.Mapping
{
    public class LocationMap : ComponentMap<LocationValueObject>
    {
        public LocationMap()
        {
            Map(x => x.Street);
            Map(x => x.City);
            Map(x => x.Province);
            Map(x => x.Country);
        }
    }
}
