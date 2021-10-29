using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Storage
{
    public class DatabaseContext : DbContext
    {
        public DbSet<FnVersion> FnVersions { get; set; }
        public DbSet<TkKey> TkKeys { get; set; }
        public DbSet<PakFile> PakFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={(Program.Configuration != null ? Program.Configuration.DatabasePath : "db.sqlite")}");
    }
}
