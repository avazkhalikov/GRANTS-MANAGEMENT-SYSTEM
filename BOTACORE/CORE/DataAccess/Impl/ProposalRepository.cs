using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.DAL;
using System.Data;
using BOTACORE.CORE.DataAccess;

namespace BOTACORE.CORE.DataAccess.Impl
{
    public class ProposalRepository : IProposalRepository
    {
        private ISqlCommands sqlcmd;
        private ProposalInfo propinfo;
        private Proposal prop;
        private Role role; 

        public ProposalRepository()
        {
            sqlcmd = SqlFactory.MSSQL();
            prop = new Proposal();
            propinfo = new ProposalInfo();
                
            // ...MoreComing. 
    
         }
        
        /// <summary>
        /// This methods for inflating the Proposal 
        /// </summary>
        /// <param name="ProposalID"></param>
        /// <returns></returns>
        public Proposal Inflate(int ProposalID)
        {
            prop.ProposalID = 1; //dr["ProposalID"]; ProposalID;
            prop.Inflated = true;
            return prop;
        }


        public Proposal InflateProposalInfo(int ProposalID)
        {
            
            //while(dr.read) all properties except permission will be rewritten multiple times the same value.

            propinfo.ProposalID = 1; 

            propinfo.Title = "MyTitle"; //dr["Status"]; 
            propinfo.PublicationStatement = "MYArea"; //dr["ProgramArea"];
            //propinfo.Inflated = true;

            //The Logic of Persisting multiple records. 
          
            role = new Role();
            role.RoleName = "Grants Manager"; 
            //propinfo.Permissions.Add(role, AccessLevel.Full);
         
            role = new Role();
            role.RoleName = "Program Officer"; 
            //propinfo.Permissions.Add(role, AccessLevel.Read);



            prop.ProposalID = 1; //dr["ProposalID"];
            prop.PropInfo = propinfo;
            prop.Inflated = true;
            
            return prop; 
        }


        public Proposal InflateProjectInfo(int ProposalID)
        {
            return prop; 
           
        }

        public Proposal InflateProjectLocation(int ProposalID)
        {
            return prop;

        }

        public Proposal InflateProjectFundingSource(int ProposalID)
        {
            return prop;

        }

        public Proposal InflateProjectKeyNotes(int ProposalID)
        {
            return prop;

        }


        //UPDATE 
        public bool UpdateProposalInfo(Proposal proposal)
        {
            //   proposal.PropInfo.Status;  
            // DBFundingSource = proposal.PropInfo.FundingSource;
            return true;
        }

        public bool UpdateProjectInfo(Proposal proposal)
        {
            //   proposal.PropInfo.Status;  
            // DBFundingSource = proposal.PropInfo.FundingSource;
            return true;
        }

        public bool UpdateProjectKeyNotes(Proposal proposal)
        {
            //   proposal.PropInfo.Status;  
            // DBFundingSource = proposal.PropInfo.FundingSource;
            return true;
        }


        public bool UpdateProjectFundingSource(Proposal proposal)
        {
            //   proposal.PropInfo.Status;  
            // DBFundingSource = proposal.PropInfo.FundingSource;
            return true;
        }

        public bool UpdateProjectLocation(Proposal proposal)
        {
            //   proposal.PropInfo.Status;  
            // DBFundingSource = proposal.PropInfo.FundingSource;
            return true;
        }
      
  
       

    }
}
