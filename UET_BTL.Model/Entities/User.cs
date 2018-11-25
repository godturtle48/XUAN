
namespace UET_BTL.Model.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Position { get; set; }
        public int? StudentID { get; set; }
        public int? TeacherID { get; set; }
    }
}
