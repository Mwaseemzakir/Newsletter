using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Contexts
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connetionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connetionString = _configuration.GetConnectionString("SQL");
        }

        public IDbConnection Connect()
        {
            return new SqlConnection(_connetionString);
        }
    }
}
