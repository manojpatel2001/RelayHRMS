using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class LeaveYearlySummaryViewModel
    {
        public int Emp_Id { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string LeaveType { get; set; }
        public int Leave_TypeId { get; set; }
        public decimal Opening { get; set; }

        // Monthly Credit
        public decimal Credit_Jan { get; set; }
        public decimal Credit_Feb { get; set; }
        public decimal Credit_Mar { get; set; }
        public decimal Credit_Apr { get; set; }
        public decimal Credit_May { get; set; }
        public decimal Credit_Jun { get; set; }
        public decimal Credit_Jul { get; set; }
        public decimal Credit_Aug { get; set; }
        public decimal Credit_Sep { get; set; }
        public decimal Credit_Oct { get; set; }
        public decimal Credit_Nov { get; set; }
        public decimal Credit_Dec { get; set; }

        // Monthly Used
        public decimal Used_Jan { get; set; }
        public decimal Used_Feb { get; set; }
        public decimal Used_Mar { get; set; }
        public decimal Used_Apr { get; set; }
        public decimal Used_May { get; set; }
        public decimal Used_Jun { get; set; }
        public decimal Used_Jul { get; set; }
        public decimal Used_Aug { get; set; }
        public decimal Used_Sep { get; set; }
        public decimal Used_Oct { get; set; }
        public decimal Used_Nov { get; set; }
        public decimal Used_Dec { get; set; }

        // Monthly Lapsed
        public decimal Lapsed_Jan { get; set; }
        public decimal Lapsed_Feb { get; set; }
        public decimal Lapsed_Mar { get; set; }
        public decimal Lapsed_Apr { get; set; }
        public decimal Lapsed_May { get; set; }
        public decimal Lapsed_Jun { get; set; }
        public decimal Lapsed_Jul { get; set; }
        public decimal Lapsed_Aug { get; set; }
        public decimal Lapsed_Sep { get; set; }
        public decimal Lapsed_Oct { get; set; }
        public decimal Lapsed_Nov { get; set; }
        public decimal Lapsed_Dec { get; set; }

        // Totals
        public decimal Total_Credit { get; set; }
        public decimal Total_Used { get; set; }
        public decimal Total_Lapsed { get; set; }
        public decimal Closing { get; set; }
    }

}
