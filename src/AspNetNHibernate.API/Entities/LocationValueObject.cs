namespace AspNetNHibernate.API.Entities
{
    public class LocationValueObject
    {
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string Province { get; set; }
        public virtual string Country { get; set; }

        public override string ToString()
        {
            return $"{Street}, {City} - {Province}, {Country}";
        }
    }
}
