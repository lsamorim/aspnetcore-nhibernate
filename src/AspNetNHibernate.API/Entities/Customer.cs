using System;

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

        public override string ToString()
        {
            return $"Id: {Id}\nFirstName: {FirstName}\nLastName: {LastName}\nPoints: {Points}\nHasGoldStatus: {HasGoldStatus}\nMemberSince: {MemberSince}\nCreditCardRating: {CreditRating.ToString()}\nAddress: {Address}";
        }
    }

    public enum CustomerCreditRating
    {
        Excellent, Good, Neutral, Poor, Terrible
    }
}
