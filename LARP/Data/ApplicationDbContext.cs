using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LARP.Models;

namespace LARP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ScriptUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<ScriptType> ScriptTypes { get; set; }
        public DbSet<ScriptTypeBind> ScriptTypeBinds { get; set; }

    }
}
