using System;

namespace AspNetNHibernate.API.Entities
{
    public class Entity
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual string UpdatedBy { get; set; }
    }
}
