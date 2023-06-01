using FarmaciaSIU.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaSIU.Data
{
    public class FarmaciaContext : DbContext
    {
        public FarmaciaContext(DbContextOptions<FarmaciaContext>options) : base(options) {
        //empty
        }

        public DbSet<Producto> Productos { get; set; }     
    }
}
