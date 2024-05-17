using EmployeeLeaveManagementApi.Entity;

namespace EmployeeLeaveManagementApi.Type
{
    public class LeaveUpdateBody
    {
        public int LeaveId { get; set; } // Primary key
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        public LeaveStatus Status { get; set; }
    }
}
