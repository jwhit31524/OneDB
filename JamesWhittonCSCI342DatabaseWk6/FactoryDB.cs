using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamesWhittonCSCI342DatabaseWk6
{
    public class FactoryDB : DbContext
    {
        public DbSet<Vehicle> Vehicle { get; set; }
    }
}
