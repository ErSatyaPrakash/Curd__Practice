using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Curd__Practice.Models
{
    public class DBLayer
    {
        SqlConnection conn=new SqlConnection("Data Source=INBOOK_X1;Initial Catalog=Curd-Practice;Integrated Security=True;Encrypt=False");
        public int ExecuteIUD(string procname, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(procname,conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }
                conn.Open();
                int res= cmd.ExecuteNonQuery();
                conn.Close();
                return res;
        }
        
            public DataTable ExecuteSelect(string procedure, SqlParameter[] parameters)
            {
                SqlCommand sqlCommand = new SqlCommand(procedure, conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                {
                    if (param.Value != null)
                        sqlCommand.Parameters.Add(param);
                }
                DataTable dt = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                dataAdapter.Fill(dt);
                return dt;
            }
           
    }
}