

namespace UET_BTL.Model.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentDetail
    {
        public int StudentDetailID { get; set; }
        public int? StudentID { get; set; }
        public int? SubjectID { get; set; }
        public int? TeacherID { get; set; }
        public string NoteSurvey { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Survey> Survey { get; set; }
    }
}
