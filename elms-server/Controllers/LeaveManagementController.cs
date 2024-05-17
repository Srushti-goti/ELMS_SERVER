using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagementApi.Data;
using EmployeeLeaveManagementApi.Entity;
using Microsoft.AspNetCore.Authorization;
using EmployeeLeaveManagementApi.Type;

namespace EmployeeLeaveManagementApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveManagementController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public LeaveManagementController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveManagement
        [HttpGet("Getall")]
        public async Task<ActionResult<IEnumerable<LeaveManagement>>> GetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveManagements
            .Include(l => l.Employee) // Include the Employee data
            .Select(l => new
            {
                l.LeaveId,
                l.EmployeeId,
                l.StartDate,
                l.EndDate,
                l.Reason,
                l.Status,
                Employee = new
                {
                    l.Employee.EmployeeId,
                    l.Employee.Name,
                    l.Employee.Age,
                    l.Employee.Birthdate,
                    l.Employee.Department,
                    l.Employee.Position,
                    l.Employee.PreviousLeave,
                    l.Employee.Email,
                    l.Employee.ContactNumber,
                    l.Employee.IsActive
                }
            })
            .ToListAsync();
            return Ok(new { message = "Leave requests retrieved successfully.", data = leaveRequests });
        }


        // GET: api/LeaveManagement/5
        [HttpGet("GetById")]
        public async Task<ActionResult<LeaveManagement>> GetLeaveRequestById(int id)
        {
            var leaveRequest = await _context.LeaveManagements.FindAsync(id);

            if (leaveRequest == null)
            {
                return NotFound(new { message = "Leave request not found." });
            }

            return Ok(new { message = "Leave request retrieved successfully.", data = leaveRequest });
        }

        // POST: api/LeaveManagement
        [HttpPost("Add")]
        public async Task<ActionResult<LeaveManagement>> AddLeaveRequest(LeaveUpdateBody leaveRequest)
        {
            int employee_id = int.Parse(User.Identity.Name);

            LeaveManagement empData = new LeaveManagement
            {
              
                EmployeeId = employee_id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
            };
            _context.LeaveManagements.Add(empData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Leave requests Added successfully.", data = leaveRequest });
        }

        // PUT: api/LeaveManagement/Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateLeaveRequest(LeaveUpdateBody leaveRequest)
        {
            var existingLeaveRequest = await _context.LeaveManagements.FindAsync(leaveRequest.LeaveId);

            if (existingLeaveRequest == null)
            {
                return NotFound(new { message = "Leave request not found." });
            }
            // Update properties of existingLeaveRequest
            existingLeaveRequest.EmployeeId = leaveRequest.EmployeeId;
            existingLeaveRequest.StartDate = leaveRequest.StartDate;
            existingLeaveRequest.EndDate = leaveRequest.EndDate;
            existingLeaveRequest.Reason = leaveRequest.Reason;
            existingLeaveRequest.Status = leaveRequest.Status;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(new { message = "Leave request updated successfully.", data = existingLeaveRequest });
        }

        // DELETE: api/LeaveManagement/5
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
      
            var leaveRequest = await _context.LeaveManagements.FirstOrDefaultAsync(x => x.LeaveId == id);
            if (leaveRequest == null)
            {
                return NotFound(new { message = "Leave request not found." });
            }

            // Soft delete: Mark the leave request as inactive
            _context.LeaveManagements.Remove(leaveRequest);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Leave requests Delete successfully.", data = id });
        }

        [HttpGet("ShowLeaveOfEmp")]
        public async Task<ActionResult<IEnumerable<LeaveManagement>>> ShowLeaveOfEmp()
        {
            string userId = User.Identity.Name;
            var leaves = await _context.LeaveManagements.Where(x => x.EmployeeId == int.Parse(userId)).ToListAsync();
            return leaves;
        }

    }
}
