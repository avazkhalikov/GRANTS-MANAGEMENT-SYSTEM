using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

/* to Use
 public static Image GetThumbnail(int photoid)

    {
       string sql = "SELECT * from bazamine ";
       SqlParameter[] p = new SqlParameter[1];
       p[0] = new SqlParameter("@id", photoid);
       SqlDataReader reader = SqlHelper.ExecuteReader(sql, p);
       while (reader.Read())
       {
        data = (byte[])reader.GetValue(0);
       }
       reader.Close();
   }
 */
namespace BOTACORE.CORE.DataAccess.DAL
{

    public class SqlHelper
    {
        private string strConn;

        public SqlHelper()
        {
            strConn = Settings1.Default.DBMLCONNECT; // GetConnectionString(); 

            //strConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; 
        }

        private string GetConnectionString()
        {
            System.Configuration.ConnectionStringSettings connectionStringsSection;
            //  if (System.Web.HttpContext.Current == null)
            {
                connectionStringsSection = System.Configuration.ConfigurationManager.ConnectionStrings["BOTADbConnection"];
            }

            return connectionStringsSection.ConnectionString;
        }


        //used for Insert UPDATE.
        public int ExecuteNonQuery(string query)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(query, cnn);
            cnn.Open();
            int retval = cmd.ExecuteNonQuery();
            cnn.Close();
            return retval;
        }

        //used for Insert UPDATE.
        public int ExecuteScalarWithOneReturnVal(string sql, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            FillParameters(cmd, p);
            cnn.Open();

            object retval;
            try
            {
                retval = cmd.ExecuteScalar();   //Execute scalar
                cnn.Close();
                return Convert.ToInt32(retval);  //return generated return ID 
            }
            //Violation of UNIQUE KEY constraint 'UQ__Picture__1DE57479'. Cannot insert duplicate key in object 'Picture'.
            catch (SqlException e)
            {
                //checks for duplicate error code! with 
                if (e.ErrorCode == -2146232060)  // && connStrSettings.ProviderName == "System.Data.SqlClient") will do this later.    
                {
                    return -1;  //duplicate                 // if you wanna make it work for all, Oracle and shit check for ProviderName
                    cnn.Close();
                }
                else
                {
                    return -2; //some other error.
                    cnn.Close();
                }
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(query, cnn);
            FillParameters(cmd, p);
            cnn.Open();
            int retval = cmd.ExecuteNonQuery();
            cnn.Close();
            return retval;
        }
        //for reading may  rows.
        public SqlDataReader ExecuteReader(string sql)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cnn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader ExecuteReader(string sql, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            FillParameters(cmd, p);
            cnn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar(string sql)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cnn.Open();
            object retval;
            
            try
            {
                retval = cmd.ExecuteScalar();   //Execute scalar
                cnn.Close();
                return 1;  //return success
            }
            //Violation of UNIQUE KEY constraint 'UQ__Picture__1DE57479'. Cannot insert duplicate key in object 'Picture'.
            catch (SqlException e)
            {
                //checks for duplicate error code! with 
                
                if (e.ErrorCode == -2146232060)  // && connStrSettings.ProviderName == "System.Data.SqlClient") will do this later.    
                {
                    return -1;  //duplicate                 // if you wanna make it work for all, Oracle and shit check for ProviderName
                    cnn.Close();
                }
                else
                {
                    return -2; //some other error.
                    cnn.Close();
                }
            }
        }
        //lightweight staff for returning select count(*)...stuff. returns 1 value.
        public object ExecuteScalar(string sql, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            FillParameters(cmd, p);
            cnn.Open();

            object retval;
            try
            {
                retval = cmd.ExecuteScalar();   //Execute scalar
                cnn.Close();
                return 1;  //return success
            }
            //Violation of UNIQUE KEY constraint 'UQ__Picture__1DE57479'. Cannot insert duplicate key in object 'Picture'.
            catch (SqlException e)
            {
                //checks for duplicate error code! with 
                if (e.ErrorCode == -2146232060)  // && connStrSettings.ProviderName == "System.Data.SqlClient") will do this later.    
                {
                    return -1;  //duplicate                 // if you wanna make it work for all, Oracle and shit check for ProviderName
                    cnn.Close();
                }
                else
                {
                    return -2; //some other error.
                    cnn.Close();
                }
            }

        }

        public DataSet ExecuteDataSet(string sql)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            cnn.Close();
            return ds;
        }

        public DataSet ExecuteDataSet(string sql, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            FillParameters(cmd, p);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            cnn.Close();
            return ds;
        }

        private void FillParameters(SqlCommand cmd, SqlParameter[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.Add(parameters[i]);
            }
        }
    }
}