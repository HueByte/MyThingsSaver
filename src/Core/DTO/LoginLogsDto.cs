using MTS.Core.Models;

namespace MTS.Core.DTO
{
    public class LoginLogsDto
    {
        public List<LoginLogModel>? Logs { get; set; }
        public int TotalCount { get; set; }
    }
}