using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Repository.Connection
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection(string name );
    }
}