namespace UET_BTL.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentSurveys",
                c => new
                    {
                        ContentSurveyID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.ContentSurveyID);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        SurveyID = c.Int(nullable: false, identity: true),
                        StudentDetailID = c.Int(),
                        ContentSurveyID = c.Int(),
                        Point = c.Double(),
                    })
                .PrimaryKey(t => t.SurveyID)
                .ForeignKey("dbo.ContentSurveys", t => t.ContentSurveyID)
                .ForeignKey("dbo.StudentDetails", t => t.StudentDetailID)
                .Index(t => t.StudentDetailID)
                .Index(t => t.ContentSurveyID);
            
            CreateTable(
                "dbo.StudentDetails",
                c => new
                    {
                        StudentDetailID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(),
                        SubjectID = c.Int(),
                        TeacherID = c.Int(),
                        NoteSurvey = c.String(),
                    })
                .PrimaryKey(t => t.StudentDetailID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .ForeignKey("dbo.Teachers", t => t.TeacherID)
                .Index(t => t.StudentID)
                .Index(t => t.SubjectID)
                .Index(t => t.TeacherID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StudentCode = c.String(),
                        Course = c.String(),
                        DateOfBirth = c.DateTime(storeType: "date"),
                        Email = c.String(),
                        UserName = c.String(),
                        PassWord = c.String(),
                    })
                .PrimaryKey(t => t.StudentID);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubjectCode = c.String(),
                        CreditNumber = c.Int(nullable: false),
                        ClassRoom = c.String(),
                        TimeTeach = c.String(),
                    })
                .PrimaryKey(t => t.SubjectID);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TeacherCode = c.String(),
                        Email = c.String(),
                        UserName = c.String(),
                        PassWord = c.String(),
                    })
                .PrimaryKey(t => t.TeacherID);
            
            CreateTable(
                "dbo.ControllerActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.String(),
                        Area = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        PassWord = c.String(),
                        Position = c.String(),
                        StudentID = c.Int(),
                        TeacherID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentDetails", "TeacherID", "dbo.Teachers");
            DropForeignKey("dbo.Surveys", "StudentDetailID", "dbo.StudentDetails");
            DropForeignKey("dbo.StudentDetails", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.StudentDetails", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Surveys", "ContentSurveyID", "dbo.ContentSurveys");
            DropIndex("dbo.StudentDetails", new[] { "TeacherID" });
            DropIndex("dbo.StudentDetails", new[] { "SubjectID" });
            DropIndex("dbo.StudentDetails", new[] { "StudentID" });
            DropIndex("dbo.Surveys", new[] { "ContentSurveyID" });
            DropIndex("dbo.Surveys", new[] { "StudentDetailID" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.ControllerActions");
            DropTable("dbo.Teachers");
            DropTable("dbo.Subjects");
            DropTable("dbo.Students");
            DropTable("dbo.StudentDetails");
            DropTable("dbo.Surveys");
            DropTable("dbo.ContentSurveys");
        }
    }
}
