using Microsoft.EntityFrameworkCore;
using AppCitas.Service.Entities;

namespace AppCitas.Service.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<AppUser> Users { get; set; }
}
