using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ObjectModel
{
    public class Staff : IStaff
    {
        public int staffID { get; set; }
        public string fullName { get; set; }
        public string passwordHash { get; set; }
        public Privelege privelege { get; set; }
    }

    public interface IStaff
    {
        int staffID { get; set; }
        string fullName { get; set; }
        string passwordHash { get; set; }
        Privelege privelege { get; set; }
    }

    public enum Privelege
    {
        Normal,
        Admin
    }
}