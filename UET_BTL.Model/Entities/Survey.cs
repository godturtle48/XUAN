

namespace UET_BTL.Model.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Survey
    {
        public int SurveyID { get; set; }
        public int? StudentDetailID { get; set; }
        public int? ContentSurveyID { get; set; }
        public double? Point { get; set; }
    
        public virtual ContentSurvey ContentSurvey { get; set; }
        public virtual StudentDetail StudentDetail { get; set; }
    }
}
