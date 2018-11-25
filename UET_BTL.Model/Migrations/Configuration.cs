namespace UET_BTL.Model.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UET_BTL.Model.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<UET_BTL.Model.UetSurveyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UET_BTL.Model.UetSurveyDbContext context)
        {
            if (!context.Users.Any(s => s.Position == "Admin"))
            {
                context.Users.Add(new Entities.User { UserName = "admin", PassWord = "admin", Position = "Admin" });
            }
            if (context.Roles.ToList().Count() == 0)
            {
                System.Collections.Generic.List<Role> roles = new List<Role>
                {
                    new Role{Position = "Admin",Area = "Admin",Controller = "Home",Action = "Index"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "StudentManager",Action = "Create"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "StudentManager",Action = "Delete"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "StudentManager",Action = "Edit"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "StudentManager",Action = "ImportStudent"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "StudentManager",Action = "Index"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SubjectManager",Action = "Delete"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SubjectManager",Action = "ImportSubject"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SubjectManager",Action = "Index"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SubjectManager",Action = "ResultSurveyEveryStudent"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SubjectManager",Action = "ShowClass"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SubjectManager",Action = "ShowResultSurvey"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SurveyManager",Action = "Create"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SurveyManager",Action = "Delete"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SurveyManager",Action = "Edit"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "SurveyManager",Action = "Index"},
                    new Role{Position = "Teacher",Area = "Member",Controller = "Teacher",Action = "Index"},
                    new Role{Position = "Teacher",Area = "Member",Controller = "Teacher",Action = "ShowClass"},
                    new Role{Position = "Teacher",Area = "Member",Controller = "Teacher",Action = "ShowInforTeacher"},
                    new Role{Position = "Teacher",Area = "Member",Controller = "Teacher",Action = "ShowListSubject"},
                    new Role{Position = "Teacher",Area = "Member",Controller = "Teacher",Action = "ShowResultSurvey"},
                    new Role{Position = "Student",Area = "Member",Controller = "Student",Action = "Index"},
                    new Role{Position = "Student",Area = "Member",Controller = "Student",Action = "ShowInforStudent"},
                    new Role{Position = "Student",Area = "Member",Controller = "Student",Action = "ShowListSubject"},
                    new Role{Position = "Student",Area = "Member",Controller = "Student",Action = "SurveySubject"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "TeacherManager",Action = "Create"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "TeacherManager",Action = "Delete"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "TeacherManager",Action = "Edit"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "TeacherManager",Action = "ImportTeacher"},
                    new Role{Position = "Admin",Area = "Admin",Controller = "TeacherManager",Action = "Index"}
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
