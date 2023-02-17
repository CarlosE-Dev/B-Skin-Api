using System;
using System.Data;
using System.Data.SqlClient;

namespace B_Skin_Api.Data.Dapper
{
    public sealed class DbSession : IDisposable
    {
        private Guid _id;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession()
        {
            _id = Guid.NewGuid();
            Connection = new SqlConnection(@"Server=localhost;Database=B_SKIN_SERVER;Trusted_Connection=True;");
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
