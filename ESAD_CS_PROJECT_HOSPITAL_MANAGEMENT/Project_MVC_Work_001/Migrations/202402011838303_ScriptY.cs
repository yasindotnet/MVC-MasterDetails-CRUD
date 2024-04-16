namespace Project_MVC_Work_001.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScriptY : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false, maxLength: 50),
                        AdmissioneDate = c.DateTime(nullable: false, storeType: "date"),
                        Age = c.Int(nullable: false),
                        MaritialStatus = c.Boolean(nullable: false),
                        Picture = c.String(),
                        DoctorId = c.Int(nullable: false),
                        DepatmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatientId)
                .ForeignKey("dbo.Departments", t => t.DepatmentId, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId)
                .Index(t => t.DepatmentId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        DoctorName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DoctorId);
            
            CreateTable(
                "dbo.PatientDetails",
                c => new
                    {
                        PatientDetailsId = c.Int(nullable: false, identity: true),
                        Test = c.Int(nullable: false),
                        Description = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatientDetailsId)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientDetails", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Patients", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Patients", "DepatmentId", "dbo.Departments");
            DropIndex("dbo.PatientDetails", new[] { "PatientId" });
            DropIndex("dbo.Patients", new[] { "DepatmentId" });
            DropIndex("dbo.Patients", new[] { "DoctorId" });
            DropTable("dbo.PatientDetails");
            DropTable("dbo.Doctors");
            DropTable("dbo.Patients");
            DropTable("dbo.Departments");
        }
    }
}
