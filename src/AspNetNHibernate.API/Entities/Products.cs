using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetNHibernate.API.Entities
{
    public class Products : Entity
    {
        public virtual string Category { get; set; }
        public virtual string Size { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Title { get; set; }
        public virtual string ArtDescription { get; set; }
        public virtual string ArtDating { get; set; }
        public virtual string ArtId { get; set; }
        public virtual string Artist { get; set; }
        public virtual DateTime ArtistBirthDate { get; set; }
        public virtual DateTime ArtistDeathDate { get; set; }
        public virtual string ArtistNationality { get; set; }
    }
}
