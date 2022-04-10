using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FrontendMVC.Models;

namespace FrontendMVC.Data
{
    public class FrontendMVCContext : DbContext
    {
        public FrontendMVCContext (DbContextOptions<FrontendMVCContext> options)
            : base(options)
        {
        }

        public DbSet<FrontendMVC.Models.Airport> Airport { get; set; }
    }
}
