﻿using System;
namespace BOTACORE.CORE.DataAccess.Impl
{
   public interface IProjectRepository
    {
        void AddFinArticleCategory(int id, int FinCatSel);
        bool CheckifSuchProjectLabelExists(string projectlabel);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.CompetitionCodeList> CompetitionCodeLists();
        int CreateNewProposal(int OrgID, string projtitle, string projectlabel);
        bool CreateProjFundingSource(BOTACORE.CORE.Domain.FundingSource _fundingSource);
        bool CreateReportPeriod(BOTACORE.CORE.Domain.ReportPeriodList item);
        bool DeleteArticle(int FinArticleID);
        bool DeleteArticleCat(int? FinArticleCatID, int? BudgetID);
        bool DeleteProjectKeyByID(int KeyID, int ProjID);
        bool DeleteProjectPermanent(int ProjID);
        bool DeleteProjFundingSource(int FundingSourceID);
        bool DeleteReportPeriod(int RepperID, int BudgetID);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.General> GetAllOrganizationList();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.Project> GetAllProjectIDs();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.Project> GetAllProjectIDs(bool GrantsOnly);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.Project> GetAllProjectIsDeleted();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.Project> getAllProjects();
        BOTACORE.CORE.Domain.Budget GetBudgetByID(int id);
        BOTACORE.CORE.Domain.Budget GetBudgetTransactionByID(int bid);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.FinArtCatList> GetCatList();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ReportPeriodList> GetFinPeriods(int BudgetID);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.FundingOrganization> GetFundingOrganizationList();
        BOTACORE.CORE.Domain.GrantType GetGrantType(int id);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.GrantTypeList> GetGrantTypeLists();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.Role> GetOrgStaffRoles();
        BOTACORE.CORE.Domain.OutComeStatement GetOutComeStatements(int ProjID);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProgramAreaList> GetProgramAreaLists();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.FundingSource> GetProjAllFundingSource(int ProjID);
        BOTACORE.CORE.Domain.Project GetProjectByID(int id);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.TheKey> GetProjectKeysByID(int ProjID);
        BOTACORE.CORE.Domain.ProjectInfo GetProjInfoOnly(int id);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProjLocationList> GetProjLocLists();
        BOTACORE.CORE.Domain.Project GetProposalInfo(int id);
        BOTACORE.CORE.Domain.ProposalInfo GetProposalInfoOnly(int id);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProposalStatusList> GetProposalStatusList();
        void InsertNewArticle(int FinArticleCatID, int BudgetID);
        void InsertNewCatName(string NewCatName);
        bool InsertNewCompetitionCodeIntoProject(int CompetitionCodeID, int ProjectID);
        bool InsertNewGrantTypeIntoProject(int GrantTypeCodeID, int ProjectID);
        bool InsertNewLFIndicatorIntoProject(int LFIndicatorID, int ProjectID);
        bool InsertNewProgramAreaIntoProject(int ProgramAreaCodeID, int ProjectID);
        bool InsertNewProjectLocationIntoProject(int ProjLocID, int ProjectID);
        bool InsertNewProposalStatusIntoProject(int ProposalStatusID, int ProjectID);
        bool InsertProjectKey(string KeyWord, int ProjectID);
        string LastEnteredProject();
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.LFIndicatorList> LFIndicatorLists();
        bool ProjectInfoUpdate(BOTACORE.CORE.Domain.ProjectInfo projInfo);
        bool ProposalInfoDelete(int id);
        bool ProposalInfoUpdate(BOTACORE.CORE.Domain.Project project, bool UpdateLabel);
        bool ProposalStatusUpdate(BOTACORE.CORE.Domain.ProposalStatus item);
        bool RestoreDeletedProject(int id);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProposalInfo> SearchProposalByID(int num);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProposalInfo> SearchProposalByLabel(string sb);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProposalInfo> SearchProposalByName(string SearchString);
        System.Collections.Generic.IEnumerable<BOTACORE.CORE.Domain.ProposalInfo> SearchProposalKeyword(string[] KeyArray);
        bool UpdateBudgetInitialAmount(int initialamt, int budgetID);
        bool UpdateOutComeStatements(BOTACORE.CORE.Domain.OutComeStatement outcome);
        bool UpdateProject(BOTACORE.CORE.Domain.Project proj);
        bool UpdateProjectBudget(BOTACORE.CORE.Domain.Budget bud);
        bool UpdateProjectLabel(BOTACORE.CORE.Domain.Project project);
        bool UpdateProjFundingSource(System.Collections.Generic.List<BOTACORE.CORE.Domain.FundingSource> fundingSourceList);
        bool UpdateReportPeriodList(System.Collections.Generic.List<BOTACORE.CORE.Domain.ReportPeriodList> repperlist);
        void UpdateReportPeriodTrans(System.Collections.Generic.List<BOTACORE.CORE.Domain.ReportPeriod> ReportPeriods);
    }
}
