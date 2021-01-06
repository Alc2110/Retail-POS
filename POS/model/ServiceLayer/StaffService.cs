using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataAccessLayer;
using Model.ObjectModel;

namespace Model.ServiceLayer
{
    public interface IStaffService
    {
        // data access layer dependency injection
        IStaffDataAccessObject dataAccessObject { get; set; }

        // data operations
        void create(IStaff staff);
        IStaff read(int staffID);
        void delete(int staffID);
        IEnumerable<IStaff> readAll();

        // event for retrieving all staff records
        event EventHandler<GetAllStaffEventArgs> GetAllStaff;
    }

    public class StaffService : IStaffService
    {
        // data access layer dependency injection
        public IStaffDataAccessObject dataAccessObject { get; set; }

        /// <summary>
        /// Constructor with data access layer injection parameter.
        /// </summary>
        /// <param name="dataAccessObject"></param>
        public StaffService(IStaffDataAccessObject dataAccessObject)
        {
            this.dataAccessObject = dataAccessObject;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StaffService()
        {
            this.dataAccessObject = new StaffDataAccessObject();
        }

        /// <summary>
        /// Create a new staff record in the database.
        /// </summary>
        /// <param name="staff"></param>
        public void create(IStaff staff)
        {
            this.dataAccessObject.addStaff(staff);

            OnGetAllStaff(new GetAllStaffEventArgs(this.dataAccessObject.getAllStaff()));
        }

        /// <summary>
        /// Retrieve a staff record from the database.
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public IStaff read(int staffID)
        {
            return this.dataAccessObject.getStaff(staffID);
        }

        /// <summary>
        /// Retrieve all staff records from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IStaff> readAll()
        {
            return this.dataAccessObject.getAllStaff();
        }

        /// <summary>
        /// Update a staff record in the database.
        /// </summary>
        /// <param name="staff"></param>
        public void update(IStaff staff)
        {
            this.dataAccessObject.updateStaff(staff);

            OnGetAllStaff(new GetAllStaffEventArgs(this.dataAccessObject.getAllStaff()));
        }

        /// <summary>
        /// Delete a staff record from the database.
        /// </summary>
        /// <param name="staffID"></param>
        public void delete(int staffID)
        {
            this.dataAccessObject.deleteStaff(staffID);

            OnGetAllStaff(new GetAllStaffEventArgs(this.dataAccessObject.getAllStaff()));
        }

        public event EventHandler<GetAllStaffEventArgs> GetAllStaff;

        protected virtual void OnGetAllStaff(GetAllStaffEventArgs args)
        {
            EventHandler<GetAllStaffEventArgs> temp = GetAllStaff;

            if (temp != null)
                GetAllStaff?.Invoke(this, args);
        }
    }

    /// <summary>
    /// Custom event arguments class for retrieving all staff records.
    /// </summary>
    public class GetAllStaffEventArgs : EventArgs
    {
        private IEnumerable<IStaff> _staff;

        public GetAllStaffEventArgs(IEnumerable<IStaff> staff)
        {
            this._staff = staff;
        }

        public IEnumerable<IStaff> getList()
        {
            return _staff;
        }
    }
}
