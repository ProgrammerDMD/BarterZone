using System;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using Project.Domain.Entities;

namespace Project.BusinessLogic.DBModel
{
    public class UserContext : DbContext
    {
        private const string ConnectionStringEnv = "MSSQL_CONNECTION";
        private const string ConnectionStringName = "DefaultConnection";
        
        public UserContext() : base(GetConnectionString())
        {
            
        }
        
        public virtual DbSet<UserTable> Users { get; set; }
        
        private static string GetConnectionString()
        {
            string connStr = Environment.GetEnvironmentVariable(ConnectionStringEnv);
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;
            }
            
            if (string.IsNullOrEmpty(connStr))
            {
                throw new InvalidOperationException($"Connection string '{ConnectionStringName}' not found in web.config and environment variable '{ConnectionStringName}' is not set.");
            }

            return connStr;
        }
    }
}