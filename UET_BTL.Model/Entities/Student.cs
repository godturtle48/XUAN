

namespace UET_BTL.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string StudentCode { get; set; }
        public string Course { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetail { get; set; }
    }
}
