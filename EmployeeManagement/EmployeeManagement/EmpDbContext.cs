using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement;

public class EmpDbContext : DbContext
{
    public EmpDbContext(DbContextOptions<EmpDbContext> options) : base(options)
    {
        
    }
}
