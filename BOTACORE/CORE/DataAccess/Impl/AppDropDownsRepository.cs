using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;

namespace BOTACORE.CORE.DataAccess.Impl
{
  public  class AppDropDownsRepository
    {

       string connectString;
       BOTADataContext db; 
       public AppDropDownsRepository()
       {
            //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();

           db = new BOTADataContext(connectString);
       }
      

      #region LFIndicator List
       public IEnumerable<LFIndicatorList> LfIndicatorList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.LFIndicatorLists
                        select ls;
           return result;
       }
      #endregion 

       #region Drop Down Budget Category

       public IEnumerable<FinArtCatListR> GetCatList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.FinArtCatListRs
                        select ls;
           return result;
       }


       public void InsertNewCatName(string NewCatName)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           FinArtCatListR fcatlist = new FinArtCatListR();
           fcatlist.FinArticleCatName = NewCatName;
           try
           {
               db.FinArtCatListRs.InsertOnSubmit(fcatlist);
               db.SubmitChanges();
           }

           catch (Exception ex)
           {

           }

       }


       public bool ArticleCatListEdit(List<FinArtCatListR> FinArtCatListR)
       {
           bool result = true;
           try
           {
               if (FinArtCatListR.Count() > 0)
               {
                   db.FinArtCatListRs.AttachAll(FinArtCatListR);
                   db.Refresh(RefreshMode.KeepCurrentValues, FinArtCatListR);
                   db.SubmitChanges();
               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }


       public bool DeleteArticleCat(int CatListID)
       {
           bool result = true;

           try
           {
               var toDelete = (from f in db.FinArtCatListRs
                               where f.FinArticleCatListID == CatListID
                               select f).FirstOrDefault();

               if (toDelete != null)
               {
                   db.FinArtCatListRs.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;

       }

       #endregion 

       #region SSPRoles
       public bool UpdateSSPRoles(IEnumerable<RolesSSPStaff> SSPRoles)
       {
           bool result = true;
           try
           {
               if (SSPRoles.Count() > 0)
               {
                   db.RolesSSPStaffs.AttachAll(SSPRoles);
                   db.Refresh(RefreshMode.KeepCurrentValues, SSPRoles);
                   db.SubmitChanges();
               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }

       public bool DeleteSSPRoles(int RoleID)
       {
           bool result = true;

           try
           {
               var toDelete = (from f in db.RolesSSPStaffs
                               where f.RoleID== RoleID
                               select f).FirstOrDefault();

               if (toDelete != null)
               {
                   db.RolesSSPStaffs.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }


       public bool InsertSSPRoles()
       {
           bool result = true;
           try
           {
               RolesSSPStaff ls = new RolesSSPStaff();
               ls.RoleName = "";
               db.RolesSSPStaffs.InsertOnSubmit(ls);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               result = false;
           }
           return result;
       } 


       #endregion 



       #region LegalStatus
       public IEnumerable<LegalStatusList> GetLegalStatusList()
      {
              var result = from ls in db.LegalStatusLists
                           select ls;
              return result;
      }

      public bool UpdateLegalStatus(IEnumerable<LegalStatusList> LegStatusList)
      {
          bool result = true;
          try
          {
              if (LegStatusList.Count() > 0)
              {
                  db.LegalStatusLists.AttachAll(LegStatusList);
                  db.Refresh(RefreshMode.KeepCurrentValues, LegStatusList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }

      public bool DeleteLegalStatus(int LegalStatID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.LegalStatusLists
                              where f.LegStatListID== LegalStatID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.LegalStatusLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertLegalStatus()
      {
          bool result = true;
          try
          {
              LegalStatusList ls = new LegalStatusList();
              ls.LegName = ""; 
  
              db.LegalStatusLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }

     #endregion 



      #region EventType
      public IEnumerable<EventType> GetEventTypeList()
      {
          var result = from ls in db.EventTypes
                       select ls;
          return result;
      }


      public bool DeleteEventTypeList(int EventTypeListID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.EventTypes
                              where f.EventTypeID == EventTypeListID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.EventTypes.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertEventTypeList()
      {
          bool result = true;
          try
          {
              EventType ls = new EventType();
              ls.EventTypeName = "";

              db.EventTypes.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool EventTypeListEdit(List<EventType> EventTypeList)
      {
          bool result = true;
          try
          {
              if (EventTypeList.Count() > 0)
              {
                  db.EventTypes.AttachAll(EventTypeList);
                  db.Refresh(RefreshMode.KeepCurrentValues, EventTypeList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion


      #region IndicatorContentLabelCategory
      public IEnumerable<IndicatorLabelContentCategory> IndicatorLabelContentCategoryList()
      {
          var result = from ls in db.IndicatorLabelContentCategories
                       select ls;
          // (from ls in db.GrantTypeLists
          //  select ls).OfType<IOptions>();
          return result;
      }


      public IndicatorLabelContentCategory GetIndicatorLabelContentCategory(int indicatorLabelContentCategoryID)
      {
          var cat = (from f in db.IndicatorLabelContentCategories
                          where f.ID == indicatorLabelContentCategoryID
                          select f).FirstOrDefault();
          return cat; 
      }


      public bool DeleteIndicatorLabelContentCategoryList(int IndicatorLabelContentCategoryListID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.IndicatorLabelContentCategories
                              where f.ID == IndicatorLabelContentCategoryListID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.IndicatorLabelContentCategories.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertIndicatorLabelContentCategoryList()
      {
          bool result = true;
          try
          {
              IndicatorLabelContentCategory ls = new IndicatorLabelContentCategory();
              ls.Text = "";
              db.IndicatorLabelContentCategories.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool IndicatorLabelContentCategoryListEdit(List<IndicatorLabelContentCategory> IndicatorLabelContentCategoryList)
      {
          bool result = true;
          try
          {
              if (IndicatorLabelContentCategoryList.Count() > 0)
              {
                  db.IndicatorLabelContentCategories.AttachAll(IndicatorLabelContentCategoryList);
                  db.Refresh(RefreshMode.KeepCurrentValues, IndicatorLabelContentCategoryList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion





      #region RegionList
      public IEnumerable<RegionList> GetRegionList()
      {
          var result = from ls in db.RegionLists
                       select ls;
          // (from ls in db.GrantTypeLists
          //  select ls).OfType<IOptions>();
          return result;
      }

      public bool DeleteRegionList(int RegionListID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.RegionLists
                              where f.DDID == RegionListID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.RegionLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
          
      }


      public bool InsertRegionList()
      {
          bool result = true;
          try
          {
              RegionList ls = new RegionList();
              ls.DDNAME = "";
              db.RegionLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool RegionListEdit(List<RegionList> RegionList)
      {
          bool result = true;
          try
          {
              if (RegionList.Count() > 0)
              {
                  db.RegionLists.AttachAll(RegionList);
                  db.Refresh(RefreshMode.KeepCurrentValues, RegionList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion



      #region GrantType
      public IEnumerable<GrantTypeList> GetGrantTypeList()
      {
          var result = from ls in db.GrantTypeLists
                       select ls;
         // (from ls in db.GrantTypeLists
                      //  select ls).OfType<IOptions>();
          return result;
      }

      public bool DeleteGrantTypeList(int GrantTypeListID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.GrantTypeLists
                              where f.GrantTypeCodeID == GrantTypeListID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.GrantTypeLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertGrantTypeList()
      {
          bool result = true;
          try
          {
              GrantTypeList ls = new GrantTypeList();
              ls.GrantTypeText = "";

              db.GrantTypeLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool GrantTypeListEdit(List<GrantTypeList> GrantTypeList)
      {
          bool result = true;
          try
          {
              if (GrantTypeList.Count() > 0)
              {
                  db.GrantTypeLists.AttachAll(GrantTypeList);
                  db.Refresh(RefreshMode.KeepCurrentValues, GrantTypeList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion


      #region Project Location
      public IEnumerable<ProjLocationList> GetProjectLocationList()
      {
          var result = from ls in db.ProjLocationLists
                       select ls;
          return result;
      }

      public bool DeleteProjectLocationList(int ProjectLocationListID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.ProjLocationLists
                              where f.ID == ProjectLocationListID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.ProjLocationLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertProjectLocationList()
      {
          bool result = true;
          try
          {
              ProjLocationList ls = new ProjLocationList();
              ls.Name = "";

              db.ProjLocationLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool ProjectLocationListEdit(List<ProjLocationList> ProjectLocationList)
      {
          bool result = true;
          try
          {
              if (ProjectLocationList.Count() > 0)
              {
                  db.ProjLocationLists.AttachAll(ProjectLocationList);
                  db.Refresh(RefreshMode.KeepCurrentValues, ProjectLocationList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion


      #region Program Area List
      public IEnumerable<ProgramAreaList> GetProgramAreaList()
      {
          var result = from ls in db.ProgramAreaLists
                       select ls;
          return result;
      }

      public bool DeleteProgramAreaList(int ProgramAreaListID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.ProgramAreaLists
                              where f.ProgramAreaCodeID == ProgramAreaListID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.ProgramAreaLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertProgramAreaList()
      {
          bool result = true;
          try
          {
              ProgramAreaList ls = new ProgramAreaList();
              ls.ProgramAreaText = "";

              db.ProgramAreaLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool ProgramAreaListEdit(List<ProgramAreaList> ProgramAreaList)
      {
          bool result = true;
          try
          {
              if (ProgramAreaList.Count() > 0)
              {
                  db.ProgramAreaLists.AttachAll(ProgramAreaList);
                  db.Refresh(RefreshMode.KeepCurrentValues, ProgramAreaList);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion


      #region Competition Code
      public IEnumerable<CompetitionCodeList> GetCompetitionCodeList()
      {
          var result = from ls in db.CompetitionCodeLists
                       select ls;
          return result;
      }

      public bool DeleteCompetitionCodeList(int ID)
      {
          bool result = true;

          try
          {
              var toDelete = (from f in db.CompetitionCodeLists
                              where f.CompetitionCodeID== ID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.CompetitionCodeLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertCompetitionCodeList()
      {
          //Insert
          bool result = true;
          try
          {
              CompetitionCodeList ls = new CompetitionCodeList();
              ls.CodeText = "";

              db.CompetitionCodeLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool CompetitionCodeListEdit(List<CompetitionCodeList> List)
      {
          bool result = true;
          try
          {
              if (List.Count() > 0)
              {
                  db.CompetitionCodeLists.AttachAll(List);
                  db.Refresh(RefreshMode.KeepCurrentValues, List);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }
      #endregion


      #region ProposalStatusList

      public IEnumerable<ProposalStatusList> GetProposalStatusList()
      {
          var result = from ls in db.ProposalStatusLists
                       select ls;
          return result;
      }

      public bool DeleteProposalStatusList(int ID)
      {
          //Delete
          bool result = true;

          try
          {
              var toDelete = (from f in db.ProposalStatusLists
                              where f.ProposalStatusID == ID
                              select f).FirstOrDefault();

              if (toDelete != null)
              {
                  db.ProposalStatusLists.DeleteOnSubmit(toDelete);
                  db.SubmitChanges();
              }
              else
              {
                  return false;
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }


      public bool InsertProposalStatusList()
      {
          //Insert
          bool result = true;
          try
          {
              ProposalStatusList ls = new ProposalStatusList();
              ls.ProposalStatusText = "";

              db.ProposalStatusLists.InsertOnSubmit(ls);
              db.SubmitChanges();
          }
          catch (Exception ex)
          {
              result = false;
          }
          return result;
      }


      public bool ProposalStatusListEdit(List<ProposalStatusList> List)
      {
          bool result = true;
          try
          {
              if (List.Count() > 0)
              {
                  db.ProposalStatusLists.AttachAll(List);
                  db.Refresh(RefreshMode.KeepCurrentValues, List);
                  db.SubmitChanges();
              }
          }
          catch (Exception ex)
          {
              result = false;
          }

          return result;
      }

      public ProposalStatusList ProposalStatusListGetItem(string ProposalStatusText)
      {
          ProposalStatusList item=null;
          var query = (from ps in db.ProposalStatusLists
                       where ps.ProposalStatusText.ToLower()==ProposalStatusText.ToLower()
                       select ps).FirstOrDefault();
          item = query;
          return item;
      }
      #endregion 





    }
}
