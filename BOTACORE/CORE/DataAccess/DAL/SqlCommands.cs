using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net.Configuration;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.DataAccess.DAL;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTACORE.CORE.DataAccess.DAL
{
   public class SqlCommands : ISqlCommands
    {

       public static string GetConnectionString()
       {
           Connection conn = new Connection();
           // string  connectString = conn.GetDirectConnString();

           return conn.GetDirectConnString();
           // return Settings1.Default.DBMLCONNECT; 
           /*
           System.Configuration.ConnectionStringSettings connectionStringsSection;
         //  if (System.Web.HttpContext.Current == null)
           {
                connectionStringsSection = System.Configuration.ConfigurationManager.ConnectionStrings["BOTADbConnection"];
           }

           return connectionStringsSection.ConnectionString;  */
       }

        public IDataReader GetProjectsForFilter(string query)
        {
            IDataReader dr = GetProjectsForFilterCommand(query).ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }


        private SqlCommand GetProjectsForFilterCommand(string query)
        {
            //TODO: Take care of this as a seperate Singelton stuff.
            string connString = GetConnectionString(); //System.Configuration.ConfigurationManager.ConnectionStrings["MyBookServerDbConnection"].ConnectionString;

            SqlConnection cn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Connection.Open();

            return cmd;
        }


       
    
        public IDataReader GetBudgetTransactionByID(int bid)
        {
            IDataReader dr = GetBudgetTransactionByIDCommand(bid).ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }


        private SqlCommand GetBudgetTransactionByIDCommand(int bid)
        {
            string query = @"SELECT Budget.ContractInitialAmt, FinArtCatList.*, FinancialArticle.*, FinArticleCategory.BudgetID as CatBudget, ReportPeriod.ReportPeriodID, ReportPeriod.Amount
FROM Budget LEFT JOIN 
     FinArticleCategory ON FinArticleCategory.BudgetID = Budget.BudgetID LEFT JOIN
     FinArtCatList ON FinArticleCategory.FinArticleCatID = FinArtCatList.FinArticleCatID  LEFT JOIN 
     FinancialArticle ON FinArticleCategory.BudgetID = FinancialArticle.BudgetID AND 
     FinArticleCategory.FinArticleCatID = FinancialArticle.FinArticleCatID LEFT JOIN
     ReportPeriod ON ReportPeriod.FinArticleID = FinancialArticle.FinArticleID  
WHERE     (Budget.BudgetID = " + bid + @")
ORDER BY  FinancialArticle.FinArticleID";

            //TODO: Take care of this as a seperate Singelton stuff.
            string connString = GetConnectionString(); //System.Configuration.ConfigurationManager.ConnectionStrings["MyBookServerDbConnection"].ConnectionString;
           
            SqlConnection cn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Connection.Open();

            return cmd;

        }




       public bool Update(string UpdateString)
       {
    //       SqlHelper sqlh = new SqlHelper();
           int retval = -1; 
           try
           {
               string connString = GetConnectionString();
               SqlConnection cnn = new SqlConnection(connString);
               SqlCommand cmd = new SqlCommand(UpdateString, cnn);
               cnn.Open();
               retval = cmd.ExecuteNonQuery();
               cnn.Close();
           }
           catch
           {
               return false;
           }

           if (retval == 1)   //return either 1 or -1. -1 error ...
               return true;
           else
               return false;                  


                              
       }

       /// <summary>
        /// @Author Avaz - returns the reader of asked username.
       /// </summary>
       /// <param name="username"></param>
       /// <returns>IDataReader</returns>
        public IDataReader GetUserByName(string username)
        {
            IDataReader dr = GetUserByNameCommand(username).ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }


        private SqlCommand GetUserByNameCommand(string username)
        {
            string query = @"SELECT * from UserT where  UserName='" + username + "'";
            //TODO: Take care of this as a seperate Singelton stuff.
            string connString = GetConnectionString(); //System.Configuration.ConfigurationManager.ConnectionStrings["MyBookServerDbConnection"].ConnectionString;
           
            SqlConnection cn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Connection.Open();

            return cmd;

        }


  
    }
}
