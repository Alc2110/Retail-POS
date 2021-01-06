using System;
using System.Collections;
using System.Collections.Generic;

namespace Model.ObjectModel
{
    public class Purchase : IPurchase
    {
        public int purchaseId { get; set; }
        public string timestamp { get; set; }
        public ICustomer customer { get; set; }
        public IStaff staff { get; set; }
        public IProduct product { get; set; }
    }

    public interface IPurchase
    {
        int purchaseId { get; set; }
        string timestamp { get; set; }
        ICustomer customer { get; set; }
        IStaff staff { get; set; }
        IProduct product { get; set; }
    }
}