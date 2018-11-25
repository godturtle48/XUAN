

namespace UET_BTL.Model.Entities
{
    using System.Collections.Generic;

    public partial class Subject
    {
        public int SubjectID { get; set; }
        public string Name { get; set; }
        public string SubjectCode { get; set; }
        public int CreditNumber { get; set; }
        public string ClassRoom { get; set; }
        public string TimeTeach { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetail { get; set; }
    }
}
