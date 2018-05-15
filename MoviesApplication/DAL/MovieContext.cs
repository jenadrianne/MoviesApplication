using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MoviesApplication.Models;

namespace MoviesApplication.DAL
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("MovieContext") {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Artist> Artist { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}