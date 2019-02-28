using System;
using System.Collections.Generic;

namespace AspNetNHibernate.API.Entities
{
    public class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int Points { get; set; }
        public virtual bool HasGoldStatus { get; set; }
        public virtual DateTime MemberSince { get; set; }
        public virtual CustomerCreditRating CreditRating { get; set; }

        public virtual LocationValueObject Address { get; set; }

        public virtual ISet<Order> Orders { get; set; } = new HashSet<Order>(0);

        public override string ToString()
        {
            int totalOrders = Orders == null ? 0 : Orders.Count;
            string ordersString = string.Empty;

            if (totalOrders > 0)
            {
                foreach(var o in Orders)
                {
                    ordersString += $"{{\n{o.ToString()}\n}}\n";
                }
            }

            return $"Id: {Id}\nFirstName: {FirstName}\nLastName: {LastName}\nPoints: {Points}\nHasGoldStatus: {HasGoldStatus}\nMemberSince: {MemberSince}\nCreditCardRating: {CreditRating.ToString()}\nAddress: {Address}\nOrders [{totalOrders}]>\n{ordersString}";
        }
    }

    public enum CustomerCreditRating
    {
        Excellent, Good, Neutral, Poor, Terrible
    }
}
