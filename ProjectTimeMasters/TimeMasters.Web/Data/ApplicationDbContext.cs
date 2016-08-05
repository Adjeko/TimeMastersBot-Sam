using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeMasters.Web.Models;
using TimeMasters.Web.Models.Logging;

namespace TimeMasters.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Log> Log { get; set; }
        /*public DbSet<TimeMasters.Web.Models.Logging.Environment> Environment { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<MetroLogVersion> MetroLogVersion { get; set; }
        public DbSet<TimeMasters.Web.Models.Logging.Exception> Exception { get; set; }
        public DbSet<ExceptionWrapper> ExceptionWrapper { get; set; }*/
    }
}
