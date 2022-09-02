using AppCitas.Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
}
