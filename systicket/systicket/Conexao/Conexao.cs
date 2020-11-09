using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace systicket.Controllers
{
    public class Conexao
    {
        private readonly IConfiguration configuration;
        private string sqlConn;
        public Conexao(IConfiguration config)
        {
            this.configuration = config;
        }

        /*
            Atenção ao modificar métodos dessa classe, eles são genericos;
        */
        

        #region GET
        public DataTable Get(string sQuery, IDictionary<object, object> paramns, CommandType cmt)
        {
            try
            {
                sqlConn = configuration.GetConnectionString("ConnString");

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    SqlCommand cmd = new SqlCommand(sQuery, conn)
                    {
                        CommandTimeout = 7200
                    };

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
                        sqlConn = string.Empty;
                    }
                }

                return dt;
            }
            catch (System.Exception)
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
                sqlConn = configuration.GetConnectionString("ConnString");

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    SqlCommand cmd = new SqlCommand(sQuery, conn)
                    {
                        CommandTimeout = 7200
                    };

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
                        sqlConn = string.Empty;
                    }

                    conn.Close();

                }

                return dt;
            }
            catch (System.Exception)
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
