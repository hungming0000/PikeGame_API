using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebBO.General.Repository.Connection
{
    public class ConnectionFactory
    {        
        public IDbConnection CreateConnection(string name )
        {
            switch (name)
            {
                case "Pgsql":
                    {
                        var ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                        return new NpgsqlConnection(ConnectionString);
                    }
                case "Mssql":
                    {
                        var ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;

                        return new SqlConnection(ConnectionString);
                    }
                default:
                    {
                        throw new Exception("name 不存在。");
                    }
            }
        }
    }
}