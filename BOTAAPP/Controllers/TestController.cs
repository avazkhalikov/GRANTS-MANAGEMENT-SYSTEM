using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using System.Text;
using BOTAMVC3.Helpers;
using System.Collections;
using System.Data;

namespace BOTAMVC3.Controllers
{
    public class TestController : BOTAController
    {
            //
        // GET: /Organization/
        // GET: /Organization/Index
        // GET: /Organization/Proposal
        OrganizationService orgservice;
        ProjectService projservice; 
        public TestController()
        {
            orgservice = new OrganizationService();
            projservice = new ProjectService(); 
        
        }

        public ActionResult Index()
        {
            return View();
        }


        //Test Values Generated.
        public ActionResult Financials()
        {
       
         FinanceResults finres = new FinanceResults(69);

         ViewData["moneyLeft"] = finres.Project_TotalCashOnHand();

         ViewData["CashOnHand"] = finres.Project_TotalCashOnHandOfLastTransferPeriod();

         ViewData["MoneySpent"] = finres.Project_TotalMoneySpentAmountFromAwardAmount();

         ViewData["MoneyTransfered"] = finres.Project_TotalMoneyTransferedFromAwardAmount();

         
        //Get All Report Periods  
         IEnumerable<ReportPeriodList> reppers = projservice.GetFinPeriods(69);
            //reportperiodID
       // ViewData["PeriodTrancheAmount"] =  finres.Project_PeriodTrancheAmount(reppers.ToList().ElementAt(0).ReportPeriods.ElementAt(0)); 
      
         return View(); 
        }




        // GET: /Organization/View
        /*public ActionResult View()
        {
            return View();
        }*/

        // GET: /Organization/View/1
        public ActionResult View(int? id)
        {

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            
            Organization _organization = orgservice.GetOrganizationOfCurrentProposal(id.Value);
            /*COMMENT:  get from model way. ViewData in View.aspx //works.
             <%= Html.DropDownListFor(
    model => model.SelectedBikeValue,
    Model.Bikes.Select(
        x => new SelectListItem {
                 Text = x.Name,
                 Value = Url.Action("Details", "Bike", new { bikeId = x.ID }),
                 Selected = x.ID == Model.ID,
        }
)) %>             
             */
            ViewData["LegalStatList"] = new SelectList(orgservice.GetLegalStatusList(),"LegStatListID","LegName");
            ViewData["ProposalID"] = id.Value;
            return View(_organization);
        }


        /*
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult View(int? id,Organization item)
        {
            return View();
        } */


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(Organization item, ICollection<Contact> Contacts,int? id, string actioncase)
        {
          
            switch (actioncase) {

              case "Save":
                            item.Contacts = ConvertBOTA.ToEntitySet(Contacts);
                            orgservice.UpdateOrganization(item, id.Value);
                            break;
              case "Delete": orgservice.DeleteOrganization(id.Value);
                             break; 
            
            }

            return RedirectToAction("View", new { id = id });

          /*  Organization _org = orgservice.GetOrganizationOfCurrentProposal(id.Value);
            if (_org != null)
            {
                item.EntityKey = _org.EntityKey;
                
            } */
            
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult test(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<form method=\"post\" name=\"form1\" id=\"form1\" action=\"/Organization/test/"+id.ToString()+"?test1=one&test2=two\">");
            sb.AppendLine("<input type=\"text\" name=\"i1\" id=\"i1\" />");
            sb.AppendLine("<input type=\"submit\" name=\"i2\" id=\"i2\" value=\"Submit\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("Test!"+id.ToString());
            return Content(sb.ToString());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult test(FormCollection form1, string test1, string test2)
        {
            string test;
            test = test1 + " " + test2;

            for (int i = 0; i < form1.Count;i++)
            {
                test += "<br />" + form1[i].ToString();
            }
            return Content(test);
        }


        public Array ReadIntoArray()
        {
            int Size = 3; //this Size is retrieved from data reader. 

            string[] arr4 = new string[Size]; // 4
            arr4[0] = "one";
            arr4[1] = "two";
            arr4[2] = "three";


            return arr4;

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult report()
        {

            DataTable dt = CreateTestDataTable();

            if (dt != null)
            {

               // ArrayList list = new ArrayList();
                //takes each dr. inside while loop and reads it into array.
                /*
                for (int i = 0; i < 3; i++)
                {
                    Array arr = ReadIntoArray();

                    if (arr != null)
                    {
                        list.Add(arr);
                    }
                } */

                ArrayList list = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    string[] arr = new string[dt.Columns.Count]; 
                    int i=0; 
                    foreach (DataColumn col in dt.Columns)
                    {

                        //Console.Write("\t " + row[col].ToString());
                        arr[i] = row[col].ToString();
                        i++; 
                    }

                    list.Add(arr);
                    i = 0; //reset
                }



                int Counter = CountForEach(list, "one", "two"); 
            }

            //Generate Report 
           
            


            return View(); 
        }


        public DataTable CreateTestDataTable()
        {
            DataTable table1 = new DataTable("Items");

            // Add columns
            DataColumn idColumn = new DataColumn("id", typeof(System.Int32));
            DataColumn itemColumn1 = new DataColumn("item1", typeof(System.String));
            DataColumn itemColumn2 = new DataColumn("item2", typeof(System.String));
            table1.Columns.Add(idColumn);
            table1.Columns.Add(itemColumn1);
            table1.Columns.Add(itemColumn2);

            // Set the primary key column.
            table1.PrimaryKey = new DataColumn[] { idColumn };

            // Add rows.
            DataRow row;

            row = table1.NewRow();
            row["id"] = 1;
            row["item1"] = "one";
            row["item2"] = "two";
            table1.Rows.Add(row);


            row = table1.NewRow();
            row["id"] = 2;
            row["item1"] = "one";
            row["item2"] = "three";
            table1.Rows.Add(row);

            row = table1.NewRow();
            row["id"] = 3;
            row["item1"] = "one";
            row["item2"] = "five";
            table1.Rows.Add(row);

            row = table1.NewRow();
            row["id"] = 4;
            row["item1"] = "one";
            row["item2"] = "two";
            table1.Rows.Add(row);


            // Accept changes.
            table1.AcceptChanges();

            return table1; 
        }


        public int CountForEach(ArrayList list, string s1, string s2)
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
