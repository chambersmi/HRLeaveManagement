using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HRDatabaseContext context) : base(context)
        {
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType) // INNER JOIN
                .ToListAsync();

            return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestsWithDetails(int id)
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id == id);

            return leaveRequests;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
        {
            var leaveRequests = await _context.LeaveRequests.Where(q => q.RequestingEmployeeId == userId)
                .Include(q => q.LeaveType)
                .ToListAsync();

            return leaveRequests;
        }
    }
}
