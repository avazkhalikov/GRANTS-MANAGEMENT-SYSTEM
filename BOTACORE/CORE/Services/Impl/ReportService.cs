using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace BOTACORE.CORE.Services.Impl
{
   public class ReportService
    {


      public int DoStats(DataTable dt, string s1, string s2)
      {
          if (dt != null)
         {
             return CountVSReport(dt, s1, s2);
         }
          return 0; 
      }


      //private DataTable ConvertDRtoDT(IDataReader dr)
      //{
      //    if (dr != null)
      //    {
      //        DataTable dt = new DataTable();
      //        dt.Load(dr);
      //        return dt;
      //    }
      //    else
      //    { 
      //       return null;           
      //    }           
      //}


       private int CountVSReport(DataTable dt, string s1, string s2)
       {

           int Counter = 0; 

           if (dt != null)
           {
               ArrayList list = new ArrayList();
               foreach (DataRow row in dt.Rows)
               {
                   string[] arr = new string[dt.Columns.Count];
                   int i = 0;
                   foreach (DataColumn col in dt.Columns)
                   {

                       //Console.Write("\t " + row[col].ToString());
                       arr[i] = row[col].ToString();
                       i++;
                   }

                   list.Add(arr);
                   i = 0; //reset
               }



              Counter = CountForEach(list, s1, s2);
           }


           return Counter; 
           
       }

       private int CountForEach(ArrayList list, string s1, string s2)
       {
           bool found1 = false;
           bool found2 = false;
           int ArrCounter = 0;
           int ListCounter = 0;

           for (int i = 0; i < list.Count; i++)
           {
               Array value = list[i] as Array;

               foreach (string str in value)
               {
                   if (str == s1) //ex: if we see "one"
                   {
                       found1 = true;
                   }

                   if (str == s2)  //ex: if we see "two"
                   {
                       found2 = true;
                   }

                   if (found1 && found2)
                   {
                       ArrCounter = 1;
                   }
               }

               ListCounter = ListCounter + ArrCounter;

               found1 = false;
               found2 = false;
               ArrCounter = 0; //we unset it for next round.
           }


           return ListCounter;
       } 

    }
}
