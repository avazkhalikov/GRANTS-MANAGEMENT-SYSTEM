using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using BOTACORE.CORE.Services.Impl;

namespace BOTACORE.CORE.DataAccess.Impl
{
    public class TemplateRepository:ITemplateRepository
    {
        private string connectString;
        private BOTADataContext db;

        public TemplateRepository()
        {
            Connection conn = new Connection();
            connectString = conn.GetDirectConnString();
            db = new BOTADataContext(connectString);
        }

        public IEnumerable<TemplateDocument> GetTemplates()
        {
            var result=from t in db.TemplateDocuments
                           select t;
            return result;
        }


        public IEnumerable<TemplateDocument> GetTemplates(int EventTypeID)
        {
            var result = from t in db.TemplateDocuments
                         where t.EventTypeID==EventTypeID
                         select t;
            return result;
        }

        public TemplateDocument GetTemplateDocument(int id)
        {
            TemplateDocument result;
            var query = (from t in db.TemplateDocuments
                         where t.TemplateDocsID == id
                         select t).FirstOrDefault();
            result = query;
            return result;
        }

        public TemplateDocument GetTemplateDocument(string TemplateName)
        {
            TemplateDocument result=null;
            try
            {
                var query = (from t in db.TemplateDocuments
                             where t.TemplateDocName.ToLower() == TemplateName.ToLower()
                             select t).First();
                result = query;
            }
            catch { }
            return result;
        }

        //public TemplateDocument GetTemplateDocument(int id)
        //{
        //    TemplateDocument result;
        //    var query = (from t in db.TemplateDocuments
        //                 where t.TemplateDocsID == id
        //                 select t).FirstOrDefault();
        //    result = query;
        //    return result;
        //}

        public bool InsertTemplateDocument(TemplateDocument item)
        {
            bool result=true;
            try
            {
                db.TemplateDocuments.InsertOnSubmit(item);
                db.SubmitChanges();
                /*item.TemplateFile.TemplateFileID = item.TemplateDocsID;
                db.TemplateFiles.InsertOnSubmit(item.TemplateFile);
                db.SubmitChanges();*/
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        public bool UpdateTemplateDocument(TemplateDocument item)
        {
            bool result = true;
            try
            {
                TemplateDocument t = new TemplateDocument();
                db.TemplateDocuments.Attach(t);
                t = item;
                db.Refresh(RefreshMode.KeepCurrentValues, t);
                db.SubmitChanges();
                if (t.TemplateFile != null)
                {
                    result = UpdateTemplateFile(t.TemplateFile);
                }

            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        public bool DeleteTemplateDocument(int id)
        {
            bool result = true;
            try
            {
                var query = (from t in db.TemplateDocuments
                             where t.TemplateDocsID == id
                             select t).FirstOrDefault();
                if (query.TemplateFile != null)
                {
                    DeleteTemplateFile(query.TemplateDocsID);
                }
                db.TemplateDocuments.DeleteOnSubmit(query);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Use UpdateTemplateDocument to Update both TemplateDocument and TemplateFile
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool UpdateTemplateFile(TemplateFile item)
        {
            bool result = true;
            try
            {
                TemplateFile t = new TemplateFile();
                db.TemplateFiles.Attach(t);
                t = item;
                db.Refresh(RefreshMode.KeepCurrentValues, t);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Use DeleteTemplateDocument to Delete both TemplateDocument and TemplateFile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTemplateFile(int id)
        {
            bool result = true;
            try
            {
                var query = (from t in db.TemplateFiles
                             where t.TemplateFileID == id
                             select t).FirstOrDefault();
                db.TemplateFiles.DeleteOnSubmit(query);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Get's fileds for mapping from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventFieldMapper> GetMapperValues()
        {
            var result = (from m in db.EventFieldMappers
                         select m);
            return result;
        }

        /// <summary>
        /// Generates Word Document from Templatename.
        /// </summary>
        /// <param name="TemplateName">Name of Template</param>
        /// <param name="EventID">ID of Event.</param>
        /// <param name="ProjectID">ID of Project.</param>
        /// <returns>String in xml format (Word document).</returns>
        public string GenerateDocumentFromTemplateName(int TemplateDocsID, int EventID, int ProjectID, int? ReportPeriodID)
        {
            string result = "";
            var Mapper = (from td in db.TemplateDocuments
                          join tf in db.TemplateFiles
                          on td.TemplateDocsID equals tf.TemplateFileID
                          where td.TemplateDocsID == TemplateDocsID
                          select tf).First();
            if (Mapper != null)
            {
                result = ReplaceTemplateKeywordsWithValues(Mapper.PhysicalAbsolutePath,EventID,ProjectID, ReportPeriodID);
            }
            return result;
        }

        public string GenerateDocumentFromTemplateName(int TemplateDocsID, int EventID, int ProjectID, string PhysicalAbsolutePathOfSite, int? ReportPeriodID)
        {
            string result = "";
            var Mapper = (from td in db.TemplateDocuments
                          join tf in db.TemplateFiles
                          on td.TemplateDocsID equals tf.TemplateFileID
                          where td.TemplateDocsID == TemplateDocsID
                          select tf).First();
            if (Mapper != null)
            {
                result = ReplaceTemplateKeywordsWithValues(PhysicalAbsolutePathOfSite+Mapper.PhysicalVirtualPath, EventID, ProjectID, ReportPeriodID);
            }
            return result;
        }

        /// <summary>
        /// Replaces template keywords with values and returns word document(xml in string).
        /// </summary>
        /// <param name="TemplatePath">Path to template from which keywords should be replaced with values.</param>
        /// <returns>String in xml format (Word document).</returns>
        public string ReplaceTemplateKeywordsWithValues(string TemplatePath, int EventID, int ProjectID, int? ReportPeriodID)
        {
            string result="";
            result = System.IO.File.ReadAllText(TemplatePath);
            List<string> keywords = GetTemplateKeywords(TemplatePath);
            IEnumerable<EventFieldMapper> MapperValues = GetMapperValues();
            string value = "";
            Dictionary<string, string> Values = new Dictionary<string, string>();
            FinanceResults finres = new FinanceResults(ProjectID);
            
            foreach (string keyword in keywords)
            {
                
                try
                {
                    string CleanedKeyword=CleanKeyword(keyword);
                    switch (CleanedKeyword.Replace("{", "").Replace("}", "").ToLower())
                    {

                        #region project properties
                        case "project id":
                            value = ProjectID.ToString();
                            break;
                        case "event completeddate":
                            if (GetProjectEvent(ProjectID, EventID).CompletedDate.HasValue)
                            {
                                DateTime d = GetProjectEvent(ProjectID, EventID).CompletedDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;
                        case "event holder":
                            if (GetProjectEvent(ProjectID, EventID).EventHolderID.HasValue)
                            {
                                SSPStaff staff = GetSSPStaff(GetProjectEvent(ProjectID, EventID).EventHolderID.Value);
                                value = staff.FirstName+" "+staff.LastName+" "+staff.MiddleName;
                            }
                            else
                            {
                                value = "";
                            }
                            break;
                        case "event type":
                            value = GetProjectEvent(ProjectID, EventID).EventType.EventTypeName;
                            break;
                        case "event scheduleddate":
                            if (GetProjectEvent(ProjectID, EventID).ScheduledDate.HasValue)
                            {
                                DateTime d = GetProjectEvent(ProjectID, EventID).ScheduledDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;
                        case "event description":
                            value = GetProjectEvent(ProjectID,EventID).EventDescription;
                            break;
                        case "project amtrequested":
                             value = GetProjectInfo(ProjectID).AmtRequested.Value.ToString();
                            break;

                        case "grant amount":
                            value = GetProjectInfo(ProjectID).AwardedAmt.Value.ToString();
                            break;
                        case "project awardedamt":
                            value = GetProjectInfo(ProjectID).AwardedAmt.Value.ToString();
                            break;
                        
                        case "project label":
                            value = GetProjectInfo(ProjectID).Project.Label;
                            break;

                        case "project accepteddate":
                            if (GetProjectInfo(ProjectID).AcceptedDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).AcceptedDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;
                        case "project startdate":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).StartDate.Value;
                                value = ((d.Day < 10) ? "0" : "")+d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;
                        
                        case "project startdate day":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).StartDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project startdate month":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).StartDate.Value;
                                value = ((d.Month < 10) ? "0" : "") + d.Month.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project startdate monthru":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                value=GetMonthRU(GetProjectInfo(ProjectID).StartDate.Value.Month);
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project startdate year":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                value = GetProjectInfo(ProjectID).StartDate.Value.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project enddate":
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).EndDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project enddate day":
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).EndDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project enddate month":
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).EndDate.Value;
                                value = ((d.Month < 10) ? "0" : "") + d.Month.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project enddate monthru":
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                value = GetMonthRU(GetProjectInfo(ProjectID).EndDate.Value.Month);
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "project enddate year":
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                value = GetProjectInfo(ProjectID).EndDate.Value.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;

                        case "grant period":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).StartDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).EndDate.Value;
                                value += " - " + ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value += "";
                            }
                            break;

                        case "project period":
                            if (GetProjectInfo(ProjectID).StartDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).StartDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            if (GetProjectInfo(ProjectID).EndDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).EndDate.Value;
                                value += " - " + ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value += "";
                            }
                            break;
                        case "project closeddate":
                            if (GetProjectInfo(ProjectID).ClosedDate.HasValue)
                            {
                                DateTime d = GetProjectInfo(ProjectID).ClosedDate.Value;
                                value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                            }
                            else
                            {
                                value += "";
                            }
                            break;

                        case "project name":
                            value = GetProposalInfo(ProjectID).TitleR;
                            break;

                        case "project nameru":
                            value = GetProposalInfo(ProjectID).TitleR;
                            break;

                        case "project nameen":
                            value = GetProposalInfo(ProjectID).TitleE;
                            break;
                        
                       

                        case "date":
                            value = ((DateTime.Now.Day < 10) ? "0" : "") + DateTime.Now.Day.ToString() + "." + ((DateTime.Now.Month < 10) ? "0" : "") + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString();
                            break;
                        case "year":
                            value = DateTime.Now.Year.ToString();
                            break;
                        case "month":
                            value = ((DateTime.Now.Month < 10) ? "0" : "") + DateTime.Now.Month.ToString();
                            break;
                        case "monthru":
                            value = GetMonthRU(DateTime.Now.Month);
                            break;
                        case "day":
                            value = ((DateTime.Now.Day < 10) ? "0" : "")+DateTime.Now.Day.ToString();
                            break;

                        #endregion

                        #region Contacts

                        case "contact fullname"://FirstName+LastName+MiddleName
                            value = GetContact(ProjectID).LastName + " " + GetContact(ProjectID).FirstName + " " + GetContact(ProjectID).MiddleName;
                            break;
                        
                        case "contact title":
                            value = GetContact(ProjectID).Title;
                            break;
                        case "contact prefix":
                            value = GetContact(ProjectID).Prefix;
                            break;
                        case "contact name":
                            value = GetContact(ProjectID).FirstName;
                            break;
                        case "contact firstname":
                            value = GetContact(ProjectID).FirstName;
                            break;
                        case "contact surname":
                            value = GetContact(ProjectID).LastName;
                            break;
                        case "contact lastname":
                            value = GetContact(ProjectID).LastName;
                            break;
                        case "contact middlename":
                            value = GetContact(ProjectID).MiddleName;
                            break;
                        case "contact email":
                            value = GetContact(ProjectID).EmailAddress;
                            break;
                        case "contact officephone":
                            value = GetContact(ProjectID).OfficePhone;
                            break;
                        case "contact homephone":
                            value = GetContact(ProjectID).HomePhone;
                            break;
                        case "contact cellphone":
                            value = GetContact(ProjectID).CellPhone;
                            break;
                        case "contact fax":
                            value = GetContact(ProjectID).FaxNumber;
                            break;
                        case "contact city":
                            value = GetContact(ProjectID).City;
                            break;
                        case "contact county":
                            value = GetContact(ProjectID).County;
                            break;
                        case "contact address":
                            value = GetContact(ProjectID).Address;
                            break;

                        #endregion
                        
                        #region SSPStaff
                        
                        //director

                        case "d fullname":
                            value = GetStaffDirector().LastName + " " + GetStaffDirector().FirstName + " " + GetStaffDirector().MiddleName;
                            break;
                        case "d name":
                            value = GetStaffDirector().FirstName;
                            break;
                        case "d firstname":
                            value = GetStaffDirector().FirstName;
                            break;
                        case "d surname":
                            value = GetStaffDirector().LastName;
                            break;
                        case "d lastname":
                            value = GetStaffDirector().LastName;
                            break;
                        case "d middlename":
                            value = GetStaffDirector().MiddleName;
                            break;
                        case "d position":
                            value = GetStaffDirector().RolesSSPStaff.RoleName;
                            break;

                        //financial director

                        case "fd fullname":
                            value = GetStaffFinDirector().LastName + " " + GetStaffFinDirector().FirstName + " " + GetStaffFinDirector().MiddleName;
                            break;
                        case "fd name":
                            value = GetStaffFinDirector().FirstName;
                            break;
                        case "fd firstname":
                            value = GetStaffFinDirector().FirstName;
                            break;
                        case "fd surname":
                            value = GetStaffFinDirector().LastName;
                            break;
                        case "fd lastname":
                            value = GetStaffFinDirector().LastName;
                            break;
                        case "fd middlename":
                            value = GetStaffFinDirector().MiddleName;
                            break;
                        case "fd position":
                            value = GetStaffFinDirector().RolesSSPStaff.RoleName;
                            break;

                            //Grant Manager
                            
                        case "gm fullname":
                            value = GetStaffGrantsManager().LastName + " " + GetStaffGrantsManager().FirstName + " " + GetStaffGrantsManager().MiddleName;
                            break;
                        case "gm name":
                            value = GetStaffGrantsManager().FirstName;
                            break;
                        case "gm firstname":
                            value = GetStaffGrantsManager().FirstName;
                            break;
                        case "gm surname":
                            value = GetStaffGrantsManager().LastName;
                            break;
                        case "gm lastname":
                            value = GetStaffGrantsManager().LastName;
                            break;
                        case "gm middlename":
                            value = GetStaffGrantsManager().MiddleName;
                            break;
                        case "gm position":
                            value = GetStaffGrantsManager().RolesSSPStaff.RoleName;
                            break;

                            //Grant Assistent

                        case "ga fullname":
                            value = GetStaffGrantsAssisstent().LastName + " " + GetStaffGrantsAssisstent().FirstName + " " + GetStaffGrantsAssisstent().MiddleName;
                            break;
                        case "ga name":
                            value = GetStaffGrantsAssisstent().FirstName;
                            break;
                        case "ga firstname":
                            value = GetStaffGrantsAssisstent().FirstName;
                            break;
                        case "ga surname":
                            value = GetStaffGrantsAssisstent().LastName;
                            break;
                        case "ga lastname":
                            value = GetStaffGrantsAssisstent().LastName;
                            break;
                        case "ga middlename":
                            value = GetStaffGrantsAssisstent().MiddleName;
                            break;
                        case "ga position":
                            value = GetStaffGrantsAssisstent().RolesSSPStaff.RoleName;
                            break;
                            
                        //program manager

                        case "pm fullname":
                            value = GetStaffProjectManager().LastName + " " + GetStaffProjectManager().FirstName + " " + GetStaffProjectManager().MiddleName;
                            break;
                        case "pm name":
                            value = GetStaffProjectManager().FirstName;
                            break;
                        case "pm firstname":
                            value = GetStaffProjectManager().FirstName;
                            break;
                        case "pm surname":
                            value = GetStaffProjectManager().LastName;
                            break;
                        case "pm lastname":
                            value = GetStaffProjectManager().LastName;
                            break;
                        case "pm middlename":
                            value = GetStaffProjectManager().MiddleName;
                            break;
                        case "pm position":
                            value = GetStaffProjectManager().RolesSSPStaff.RoleName;
                            break;

                        //program assistent

                        case "pa fullname":
                            value = GetStaffProjectAssisstent().LastName + " " + GetStaffProjectAssisstent().FirstName + " " + GetStaffProjectAssisstent().MiddleName;
                            break;
                        case "pa name":
                            value = GetStaffProjectAssisstent().FirstName;
                            break;
                        case "pa firstname":
                            value = GetStaffProjectAssisstent().FirstName;
                            break;
                        case "pa surname":
                            value = GetStaffProjectAssisstent().LastName;
                            break;
                        case "pa lastname":
                            value = GetStaffProjectAssisstent().LastName;
                            break;
                        case "pa middlename":
                            value = GetStaffProjectAssisstent().MiddleName;
                            break;
                        case "pa position":
                            value = GetStaffProjectAssisstent().RolesSSPStaff.RoleName;
                            break;

                        //accountant

                        case "a fullname":
                            value = GetStaffAccountant().LastName + " " + GetStaffAccountant().FirstName + " " + GetStaffAccountant().MiddleName;
                            break;
                        case "a name":
                            value = GetStaffAccountant().FirstName;
                            break;
                        case "a firstname":
                            value = GetStaffAccountant().FirstName;
                            break;
                        case "a surname":
                            value = GetStaffAccountant().LastName;
                            break;
                        case "a lastname":
                            value = GetStaffAccountant().LastName;
                            break;
                        case "a middlename":
                            value = GetStaffAccountant().MiddleName;
                            break;
                        case "a position":
                            value = GetStaffAccountant().RolesSSPStaff.RoleName;
                            break;

                        //system administrator

                        case "sa fullname":
                            value = GetStaffAdmin().LastName + " " + GetStaffAdmin().FirstName + " " + GetStaffAdmin().MiddleName;
                            break;
                        case "sa name":
                            value = GetStaffAdmin().FirstName;
                            break;
                        case "sa firstname":
                            value = GetStaffAdmin().FirstName;
                            break;
                        case "sa surname":
                            value = GetStaffAdmin().LastName;
                            break;
                        case "sa lastname":
                            value = GetStaffAdmin().LastName;
                            break;
                        case "sa middlename":
                            value = GetStaffAdmin().MiddleName;
                            break;
                        case "sa position":
                            value = GetStaffAdmin().RolesSSPStaff.RoleName;
                            break;
                        #endregion

                        #region bank

                        case "bank":
                            value = GetBankInfo(ProjectID).bank;
                            break;
                        case "bank address1":
                            value=GetBankInfo(ProjectID).Address1;
                            break;
                        case "bank address2":
                            value=GetBankInfo(ProjectID).Address2;
                            break;
                        case "bank city":
                            value=GetBankInfo(ProjectID).City;
                            break;
                        case "bank state":
                            value=GetBankInfo(ProjectID).State;
                            break;
                        case "bank zip":
                            value=GetBankInfo(ProjectID).Zip;
                            break;
                        case "bank country":
                            value=GetBankInfo(ProjectID).Country;
                            break;
                        case "bank name":
                            value=GetBankInfo(ProjectID).BankName;
                            break;
                        case "bank accountnumber":
                            value=GetBankInfo(ProjectID).AccountNumber;
                            break;
                        case "bank sortcode":
                            value=GetBankInfo(ProjectID).SortCode;
                            break;
                        case "bank correspond":
                            value=GetBankInfo(ProjectID).Correspond;
                            break;
                        case "bank abaswift":
                            value=GetBankInfo(ProjectID).ABASwift;
                            break;
                        case "bank swift":
                            value=GetBankInfo(ProjectID).swift;
                            break;
                        case "bank tin":
                            value = GetBankInfo(ProjectID).TIN;
                            break;

                        #endregion

                        #region ReportPeriod

                        case "report period":
                            if (ReportPeriodID.HasValue)
                            {
                                if (GetReportPeriodList(ProjectID, ReportPeriodID.Value).PeriodStart.HasValue)
                                {
                                    DateTime d = GetReportPeriodList(ProjectID, ReportPeriodID.Value).PeriodStart.Value;
                                    value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                                }
                                else
                                {
                                    value = "";
                                }
                                if (GetReportPeriodList(ProjectID, ReportPeriodID.Value).PeriodEnd.HasValue)
                                {
                                    DateTime d = GetReportPeriodList(ProjectID, ReportPeriodID.Value).PeriodEnd.Value;
                                    value += " - " + ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                                }
                                else
                                {
                                    value += "";
                                }

                            }
                            break;
                        case "report periodstart":  //TODO: Create Seperate Case for Previous ReportPeriod
                            if (ReportPeriodID.HasValue)
                            {
                                //ReportPeriodListR rep = GetReportPeriodList(ProjectID, ReportPeriodID.Value);   
                                ReportPeriodListR rep = GetPreviousReportPeriod(ProjectID, ReportPeriodID.Value); //TEMP FIX
                                if (rep.PeriodStart.HasValue)
                                {
                                    DateTime d = rep.PeriodStart.Value;
                                    value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                                }
                                else
                                {
                                    value = "";
                                }
                            }
                            break;
                        case "report periodend"://TODO: Create Seperate Case for Previous ReportPeriod
                            if (ReportPeriodID.HasValue)
                            {
                               // ReportPeriodListR rep = GetReportPeriodList(ProjectID, ReportPeriodID.Value);
                                ReportPeriodListR rep = GetPreviousReportPeriod(ProjectID, ReportPeriodID.Value);
                                if (rep.PeriodEnd.HasValue)
                                {
                                    DateTime d = rep.PeriodEnd.Value;
                                    value = ((d.Day < 10) ? "0" : "") + d.Day.ToString() + "." + ((d.Month < 10) ? "0" : "") + d.Month.ToString() + "." + d.Year.ToString();
                                }
                                else
                                {
                                    value = "";
                                }
                            }
                            break;
                        case "report paymentamount":
                            if (ReportPeriodID.HasValue)
                            {
                                ReportPeriodListR rep = GetReportPeriodList(ProjectID, ReportPeriodID.Value);
                                if (rep.PaymentAmount.HasValue)
                                {
                                    value = rep.PaymentAmount.Value.ToString();
                                }
                            }
                            break;
                        case "report paymentdate":
                            if (ReportPeriodID.HasValue)
                            {
                                ReportPeriodListR rep = GetReportPeriodList(ProjectID, ReportPeriodID.Value);
                                if (rep.PaymentDate.HasValue)
                                {
                                    value = ((rep.PaymentDate.Value.Day < 10) ? "0" : "") + rep.PaymentDate.Value.Day.ToString() + "." + ((rep.PaymentDate.Value.Month < 10) ? "0" : "") + rep.PaymentDate.Value.Month.ToString() + "." + rep.PaymentDate.Value.Year.ToString();
                                }
                            }
                            break;
                        case "report periodtitle":
                            if (ReportPeriodID.HasValue)
                            {
                                ReportPeriodListR rep = GetReportPeriodList(ProjectID, ReportPeriodID.Value);
                                value = rep.PeriodTitle;
                            }
                            break;

                        #endregion


                        #region adress of organization
                        case "org address":
                            value = GetAddress(ProjectID).LegalAddress;
                            break;
                        case "org locationhint":
                            value = GetAddress(ProjectID).LocationHint;
                            break;
                        case "org country":
                            value = GetAddress(ProjectID).Country;
                            break;
                        case "org area":
                            value = GetAddress(ProjectID).Area;
                            break;
                        case "org city":
                            value = GetAddress(ProjectID).City;
                            break;
                        case "org region":
                            value = GetAddress(ProjectID).Region;
                            break;
                        case "org village":
                            value = GetAddress(ProjectID).Village;
                            break;
                        case "org phone":
                            value = GetAddress(ProjectID).Phone;
                            break;
                        case "org fax":
                            value = GetAddress(ProjectID).Fax;
                            break;
                        case "org email":
                            value = GetAddress(ProjectID).email;
                            break;
                        case "org website":
                            value = GetAddress(ProjectID).website;
                            break;
                        case "org name":
                            value = GetOrganizaion(ProjectID).General.NameRu;
                            break;
                        case "org nameru":
                            value = GetOrganizaion(ProjectID).General.NameRu;
                            break;
                        case "org nameen":
                            value = GetOrganizaion(ProjectID).General.Name;
                            break;

                        #endregion

                        case "total spent amount":
                            value = finres.Project_TotalMoneySpentAmountFromAwardAmount().ToString();
                            
                            break;
                        case "final left amount":
                            value = finres.Project_TotalAmountLeftFromAwardAmount().ToString();
                            break;
                        case "total transferred amount":
                            value = finres.Project_TotalMoneyTransferedFromAwardAmount().ToString();
                            break;
                        case "cash on hand":
                            value = finres.Project_TotalCashOnHand().ToString();
                            break;
                        case "next tranche amount":
                            if (ReportPeriodID.HasValue)
                            {
                                ReportPeriodR tperiod = (from p in db.ReportPeriodRs
                                                        where p.ReportPeriodID == ReportPeriodID.Value
                                                        select p).FirstOrDefault();

                                value = finres.Project_PeriodTrancheAmount(tperiod).ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            break;
                        case "first tranche amount":
                            BudgetService budgetservice = new BudgetService(); 
                            IEnumerable<ReportPeriodListR> reppers = budgetservice.GetFinPeriods(ProjectID);
                            value=finres.Project_PeriodTrancheAmount(reppers.ToList().ElementAt(0).ReportPeriodRs.ElementAt(0)).ToString(); 
                            break;

                        default:
                            value=keyword;
                            break;
                    }
                }
                catch (Exception ex) { value = ""; }
                if (string.IsNullOrEmpty(value)) {value="";}
                Values.Add(keyword, value.ToString());
            }
            foreach (KeyValuePair<string,string> item in Values)
            {
                result = result.Replace(item.Key, item.Value);
            }
            return result;
        }

        #region private helpers

        private string GetMonthRU(int Month)
        {
            string result = "";
            switch (Month)
            {
                case 1:
                    result = "январь";
                    break;
                case 2:
                    result = "февраль";
                    break;
                case 3:
                    result = "март";
                    break;
                case 4:
                    result = "апрель";
                    break;
                case 5:
                    result = "май";
                    break;
                case 6:
                    result = "июнь";
                    break;
                case 7:
                    result = "июль";
                    break;
                case 8:
                    result = "август";
                    break;
                case 9:
                    result = "сентябрь";
                    break;
                case 10:
                    result = "октябрь";
                    break;
                case 11:
                    result = "ноябрь";
                    break;
                case 12:
                    result = "декабрь";
                    break;

            }
            return result;
        }

        private Organization GetOrganizaion(int ProjectID)
        {
            Organization result;
            var query = (from o in db.Organizations
                         join p in db.Projects
                             on o.OrgID equals p.OrgID
                         where p.ProjectID == ProjectID
                         select o).FirstOrDefault();
            result = query;
            return result;
        }

        private Address GetAddress(int ProjectID)
        {
            Address result;
            var query = (from a in db.Addresses
                         join p in db.Projects
                             on a.OrgID equals p.OrgID
                         where p.ProjectID == ProjectID && a.isLegalAddress==true
                         select a).FirstOrDefault();
            result = query;
            return result;
        }

        private ReportPeriodListR GetPreviousReportPeriod(int ProjectID, int ReportPeriodID)
        {
            var query = (from r in db.ReportPeriodListRs
                         where r.BudgetID == ProjectID
                         select r).OrderBy(r=>r.PeriodStart);

            ReportPeriodListR previous = new ReportPeriodListR(); 
            foreach (ReportPeriodListR reportperiod in query)
            {
                if (reportperiod.ReportPeriodID == ReportPeriodID)
                {
                    return reportperiod; // previous; //found **what the fuck is about previous? ***
                }

                previous = reportperiod;

            }
            
            return query.FirstOrDefault();  //if no previous then it must be the first period.
        }

        private ReportPeriodListR GetReportPeriodList(int ProjectID, int ReportPeriodID)
        {
            ReportPeriodListR result;
        
            var query = (from r in db.ReportPeriodListRs
                         where r.BudgetID == ProjectID && r.ReportPeriodID == ReportPeriodID
                         select r).FirstOrDefault();
            result = query;
            return result;
        }

        private BankInfo GetBankInfo(int ProjectID)
        {
            BankInfo result;
            var query = (from b in db.BankInfos
                         where b.BankInfoID == ProjectID
                         select b).FirstOrDefault();
            result = query;
            return result;
        }


        private SSPStaff GetStaffDirector()
        {
            SSPStaff result;
            var query=(from s in db.SSPStaffs
                           join r in db.RolesSSPStaffs
                           on s.RoleID equals r.RoleID
                           where r.RoleName.ToLower()=="Director".ToLower()
                           select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffFinDirector()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Financial Director".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffGrantsManager()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Grants Manager".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffGrantsAssisstent()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Grants Assisstent".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffProjectManager()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Project Manager".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffProjectAssisstent()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Project Assisstent".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffAccountant()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Accountant".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetStaffAdmin()
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         join r in db.RolesSSPStaffs
                         on s.RoleID equals r.RoleID
                         where r.RoleName.ToLower() == "Admin".ToLower()
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private SSPStaff GetSSPStaff(int SSPStaffID)
        {
            SSPStaff result;
            var query = (from s in db.SSPStaffs
                         where s.SSPStaffID == SSPStaffID
                         select s).FirstOrDefault();
            result = query;
            return result;
        }

        private ProposalInfo GetProposalInfo(int ProjectID)
        {
            ProposalInfo result;
            var query = (from p in db.ProposalInfos
                         where p.ProjectID == ProjectID
                         select p).FirstOrDefault();
            result = query;
            return result;
        }

        private ProjectEvent GetProjectEvent(int ProjectID, int EventID)
        {
            ProjectEvent result;
            var query = (from v in db.ProjectEvents
                       where v.EventID == EventID && v.ProjectID == ProjectID
                       select v).FirstOrDefault<ProjectEvent>();
            result = query;
            return result;
        }

        private ProjectInfo GetProjectInfo(int ProjectID)
        {
            ProjectInfo result;
            var query=(from v in db.ProjectInfos
             where v.ProjectInfoID == ProjectID
             select v).FirstOrDefault<ProjectInfo>();
            result = query;
            return result;
        }

        private Contact GetContact(int ProjectID)
        {
            Contact result;
            var query=(from c in db.Contacts
             join p in db.Projects
             on c.OrgID equals p.OrgID
             where p.ProjectID == ProjectID
             select c).FirstOrDefault();
            result = query;
            return result;
        }

        
        private string CleanKeyword(string keyword)
        {
            string result=keyword;
            while (result.Contains("<") & result.Contains(">"))
            {
                //string keyword = result.Substring(result.IndexOf("<"), result.IndexOf(">") - result.IndexOf("<") + 1);
                result = result.Remove(result.IndexOf("<"), (result.IndexOf(">") - result.IndexOf("<") + 1));
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Get's keywords in template document.
        /// </summary>
        /// <param name="path">Path to word template</param>
        /// <returns>List of Keywords</returns>
        public List<string> GetTemplateKeywords(string path)
        {
            List<string> result=new List<string>();
            //XmlTextReader reader = new XmlTextReader(path);
            string reader=System.IO.File.ReadAllText(path);
            string param = "";
          //  while (reader.Read())
           // {//убрать XML reader и считывать из строки файлы.
               // if (!((reader.NodeType == XmlNodeType.Whitespace) | (reader.NodeType == XmlNodeType.SignificantWhitespace) | (reader.NodeType == XmlNodeType.Element) | (reader.NodeType == XmlNodeType.EndElement)))
                //{
                    if (reader.Contains("{") & reader.Contains("}"))
                    {
                        param = reader;
                        while (param.Contains("{") & param.Contains("}"))
                        {
                            string keyword = param.Substring(param.IndexOf("{"), param.IndexOf("}") - param.IndexOf("{") + 1);
                            param = param.Remove(param.IndexOf("{"), (param.IndexOf("}") - param.IndexOf("{") + 1));
                            //keyword = CleanKeyword(keyword);
                            //keyword must be not cleaned from word's tags
                            if (result.Contains(keyword))
                            {
                                continue;
                            }
                            else
                            {
                                result.Add(keyword);
                            }
                            
                        }
                    }
                //}
           // }
            return result;
        }
    }
}
