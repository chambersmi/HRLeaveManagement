using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
{
    Task<List<LeaveRequest>> GetLeaveRequestsWithDetails();
    Task<LeaveRequest> GetLeaveRequestsWithDetails(int id);
    Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId);
}
