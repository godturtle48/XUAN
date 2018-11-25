using System.Data.Entity;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model
{
    public class UetSurveyDbContext : DbContext
    {
        public UetSurveyDbContext() : base("UetSurveyDbConnectionstring")
        {
                      
        }
        public DbSet<ContentSurvey> ContentSurveys { get; set; }
        public DbSet<Student> Students{ get; set; }
        public DbSet<StudentDetail> StudentDetails { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
