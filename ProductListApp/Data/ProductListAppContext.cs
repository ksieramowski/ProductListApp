using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductListApp.Models;

namespace ProductListApp.Data
{
    public class ProductListAppContext : IdentityDbContext<User>
    {
        public ProductListAppContext (DbContextOptions<ProductListAppContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Product> ProductLists { get; set; } = default!;
    }
}
