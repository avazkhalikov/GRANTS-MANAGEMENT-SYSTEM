using System;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.DataAccess
{
   public interface IProposalRepository
    {
        Proposal Inflate(int ProposalID);
        Proposal InflateProjectFundingSource(int ProposalID);
        Proposal InflateProjectInfo(int ProposalID);
        Proposal InflateProjectKeyNotes(int ProposalID);
        Proposal InflateProjectLocation(int ProposalID);
        Proposal InflateProposalInfo(int ProposalID);
        bool UpdateProjectFundingSource(Proposal proposal);
        bool UpdateProjectInfo(Proposal proposal);
        bool UpdateProjectKeyNotes(Proposal proposal);
        bool UpdateProjectLocation(Proposal proposal);
        bool UpdateProposalInfo(Proposal proposal);
    }
}
