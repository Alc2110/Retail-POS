using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;

namespace POS.View
{
    public interface IStaffView
    {
        void populateStaff(IEnumerable<IStaff> staff);

        // events
        event EventHandler populateStaffRequest;
        event EventHandler<AddStaffRequestEventArgs> addStaffRequest;
        event EventHandler<DeleteStaffRequestEventArgs> deleteStaffRequest;
    }

    public class AddStaffRequestEventArgs : EventArgs
    {
        private IStaff _staff;

        public AddStaffRequestEventArgs(IStaff staff)
        {
            this._staff = staff;
        }

        public IStaff getStaff() { return this._staff; }
    }

    public class DeleteStaffRequestEventArgs : EventArgs
    {
        private int _staffId;

        public DeleteStaffRequestEventArgs(int staffId)
        {
            this._staffId = staffId;
        }

        public int getStaffId() { return this._staffId; }
    }
}
