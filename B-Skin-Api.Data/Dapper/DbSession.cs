using MySql.Data.MySqlClient;
using System;
using System.Data;

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
            Connection = new MySqlConnection(@"Server=localhost;Initial Catalog=BSKIN_DB;User=root;Password=root;");
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
