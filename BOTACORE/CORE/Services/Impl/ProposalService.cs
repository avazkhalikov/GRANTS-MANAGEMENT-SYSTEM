using System;
using System.Collections.Generic;
using System.Text;
using BOTACORE.CORE.DataAccess;
using StructureMap;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTACORE.CORE.Services.Impl
{
    public class ProposalService : BOTACORE.CORE.Services.Impl.IProposalService
    {
       private IProposalRepository propRep;

       public ProposalService()
       {
                     
           propRep = RepositoryFactory.ProposalRepository(); 
                 
       }


       public Proposal GetProposalByID(int ProposalID)
       {
          
          Proposal proposal = propRep.Inflate(ProposalID);

          return proposal;
           //Do Some Logic here ...
          
       }

       
       public ProposalInfo  GetProposalInfoByID(int ProposalID)
       {

           Proposal proposal = propRep.InflateProposalInfo(ProposalID);

           return proposal.PropInfo;

           //Now Do Some Logic with Proposal!

       }

    }
}
