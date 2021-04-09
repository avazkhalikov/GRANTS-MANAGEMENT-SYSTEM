using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
    public class Proposal
    {
        //When asked for Proposal From Repository It returns Empty Non Inflated Instances Only.
        // I must ask to Inflate them if i want to use any of them. Since I will be using them one at a time.
        
        //So Proposal Constructor will Return only the Proposal Inflated object with other empty instances.
        //Proposal.PropInfo.inflate(); 

       private ProposalInfo _PropInfo;
       private ProjectInfo _ProjInfo;
       private ProjectLocation _ProjectLocation;
       private FundingSource _FundingSource;
       
       private bool _Inflated; 



       public bool Inflated
       {
           get
           {
               return this._Inflated;
           }
           set
           {
              
                  this._Inflated = value;

           }
       }

       private int _ProposalID;
       public int ProposalID
       {
           get
           {
               return this._ProposalID;
           }
           set
           {
               if ((this._ProposalID != value))
               {
                   this._ProposalID = value;

               }
           }
       }


       public ProjectInfo ProjInfo
       {
           get
           {
               return this._ProjInfo;
           }
           set
           {
               if ((this._ProjInfo != value))
               {
                   this._ProjInfo = value;

               }
           }
       }

       public ProjectLocation ProjectLocation
       {
           get
           {
               return this._ProjectLocation;
           }
           set
           {
               if ((this._ProjectLocation != value))
               {
                   this._ProjectLocation = value;

               }
           }
       }

       public FundingSource FundingSource
       {
           get
           {
               return this._FundingSource;
           }
           set
           {
               if ((this._FundingSource != value))
               {
                   this._FundingSource = value;

               }
           }
       }

       public ProposalInfo PropInfo
       {
           get
           {
               return this._PropInfo;
           }
           set
           {
               if ((this._PropInfo != value))
               {
                   this._PropInfo = value;

               }
           }
       }

      
        //DO PUBLIC Properties NEXT: 
        //FIGURE OUT HOW IT IS GOING TO HOLD IT!
        
        
        
        //Its Constructor will user Factory to get all Other Instances or Repository Will DO iT OR WHAT????
       // See Andrew's Code or Yours ....For reference or others code!!!


    }
}
