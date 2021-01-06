using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using Model.ServiceLayer;
using POS.View;

namespace POS.Controller
{
    public interface IStaffController
    {
        // view dependency injection
        IList<IStaffView> views { get; }

        // service layer dependency injection
        IStaffService service { get; set; }
    }

    public class StaffController : IStaffController
    {
        // view dependency injection
        public IList<IStaffView> views { get; }

        /// <summary>
        /// Add a view to the list of observers, and subscribe to their events.
        /// </summary>
        /// <param name="view"></param>
        public void addObserver(IStaffView view)
        {
            view.addStaffRequest += V_addStaffRequest;
            view.populateStaffRequest += V_populateStaffRequest;
            view.deleteStaffRequest += V_deleteStaffRequest;

            views.Add(view);
        }

        // service layer dependency injection
        public IStaffService service { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StaffController()
        {
            this.views = new List<IStaffView>();

            this.service = new StaffService();
        }

        /// <summary>
        /// Event handler for delete staff requests from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V_deleteStaffRequest(object sender, DeleteStaffRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event handler for populate requests from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V_populateStaffRequest(object sender, EventArgs e)
        {
            // ask the Model for a list of all staff records
            List<IStaff> allStaff = this.service.readAll().ToList();

            // update the views
            foreach (var v in views)
                v.populateStaff(allStaff);
        }

        /// <summary>
        /// Event handler for add staff requests from views.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V_addStaffRequest(object sender, AddStaffRequestEventArgs e)
        {

        }
    }
}
