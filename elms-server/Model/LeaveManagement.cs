using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace EmployeeLeaveManagementApi.Entity
{
    public class LeaveManagement
    {
        [Key]
        public int LeaveId { get; set; }

        public int EmployeeId { get; set; }
    
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public virtual Employee Employee { get; set; }
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
