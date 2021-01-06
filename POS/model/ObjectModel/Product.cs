using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectModel
{
    public interface IProduct
    {
        long productId { get; set; }
        string productNumber { get; set; }
        string description { get; set; }
        int quantity { get; set; }
        float price { get; set; }
    }

    public class Product : IProduct
    {
        public long productId { get; set; }
        public string productNumber { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
    }
}
