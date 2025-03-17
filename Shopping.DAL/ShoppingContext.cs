using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopping.DAL.Entities;

namespace Shopping.DAL
{
    public class ShoppingContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Article> Articles {get; set;}
    }
}
