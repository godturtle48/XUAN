
namespace UET_BTL.Model.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Teacher
    {
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public string TeacherCode { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    
        public virtual ICollection<StudentDetail> StudentDetail { get; set; }
    }
}
