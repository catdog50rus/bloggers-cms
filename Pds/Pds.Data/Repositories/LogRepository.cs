using Pds.Data.Entities;
using Pds.Data.Repositories.Interfaces;

namespace Pds.Data.Repositories
{
    public class LogRepository : RepositoryBase<LogRecord>, ILogRepository
    {
        public LogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}