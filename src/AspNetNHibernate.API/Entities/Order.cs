using System;

namespace AspNetNHibernate.API.Entities
{
    public class Order
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime OrderedAt { get; set; }
        public virtual DateTime? Shipped { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual LocationValueObject ShipTo { get; set; }

        public Order()
        {
            OrderedAt = DateTime.UtcNow;
            Shipped = null;
        }

        public override string ToString()
        {
            return $"Id: {Id}\nOrderedAt: {OrderedAt}\n\nSHipTo: {ShipTo}\nShippedAt: {Shipped}";
        }
    }
}
