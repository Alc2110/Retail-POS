using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectModel
{
    public class Customer : ICustomer
    {
        public int customerId { get; set; }
        public string fullName { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string city { get; set; }
        public States state { get; set; }
        public int Postcode { get; set; }
        public IEnumerable<IPurchase> purchases { get; set; }
    }

    public interface ICustomer
    {
        int customerId { get; set; }
        string fullName { get; set; }
        string address { get; set; }
        string phoneNumber { get; set; }
        string email { get; set; }
        string city { get; set; }
        States state { get; set; }
        int Postcode { get; set; }
        IEnumerable<IPurchase> purchases { get; set; }
    }

    public enum States
    {
        NSW,
        Qld,
        Tas,
        ACT,
        Vic,
        SA,
        WA,
        NT,
        Other,
        Default
    }
}