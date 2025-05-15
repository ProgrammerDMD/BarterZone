using System;
using System.Configuration;
using System.Data.Entity;
using Project.Domain.Entities;

namespace Project.BusinessLogic.DBModel
{
    public class DatabaseContext : DbContext
    {
        private const string ConnectionStringEnv = "MSSQL_CONNECTION";
        private const string ConnectionStringName = "DefaultConnection";

        public DatabaseContext() : base(GetConnectionString())
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        private static string GetConnectionString()
        {
            var connStr = Environment.GetEnvironmentVariable(ConnectionStringEnv);
            if (string.IsNullOrEmpty(connStr))
                connStr = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;

            if (string.IsNullOrEmpty(connStr))
                throw new InvalidOperationException(
                    $"Connection string '{ConnectionStringName}' not found in web.config and environment variable '{ConnectionStringName}' is not set.");

            return connStr;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}