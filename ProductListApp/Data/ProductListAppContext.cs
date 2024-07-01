using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductListApp.Models;

namespace ProductListApp.Data
{
    public class ProductListAppContext : DbContext
    {
        public ProductListAppContext (DbContextOptions<ProductListAppContext> options)
            : base(options)
        {
        }

        public DbSet<ProductListApp.Models.Product> Product { get; set; } = default!;
    }
}
