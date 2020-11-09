using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace systicket.Controllers
{
    public class Conexao
    {
        private readonly IConfiguration configuration;
        public Conexao(IConfiguration config)
        {
            this.configuration = config;
        }
        
        #region GET
        public DataTable Get(string sQuery, IDictionary<object, object> paramns, CommandType cmt)
        {
            try
            {
                string sqlConn = configuration.GetConnectionString("ConnString");

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    SqlCommand cmd = new SqlCommand(sQuery, conn);

                    cmd.CommandTimeout = 7200;

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    cmd.CommandType = cmt;

                    foreach (var item in paramns)
                    {
                        cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                    }

                    cmd.CommandTimeout = 7200;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Load(reader);
                    using (conn)
                    {
                        conn.Close();
                    }
                }

                return dt;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion GET

        #region POST
        public DataTable Post(string sQuery, IDictionary<object, object> paramns, CommandType cmt)
        {
            try
            {
                string sqlConn = configuration.GetConnectionString("ConnString");

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    SqlCommand cmd = new SqlCommand(sQuery, conn);

                    cmd.CommandTimeout = 7200;

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    cmd.CommandType = cmt;

                    foreach (var item in paramns)
                    {
                        cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                    }

                    cmd.CommandTimeout = 7200;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Load(reader);
                    using (conn)
                    {
                        conn.Close();
                    }

                    conn.Close();

                }

                return dt;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion POST

        #region PUT

        #endregion PUT

        #region DELETE

        #endregion DELETE      
    }
}
