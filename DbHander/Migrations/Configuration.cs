namespace DbHander.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;
    using System.Data.Entity.Validation;
    using System.Linq;
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            //SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
            // Database.SetInitializer<DbContext>(null);
        }

        protected override void Seed(DataContext context)
        {          

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            const string name = "Samarpratap.singh@gmail.com";
            const string password = "Gurdit@s1ngh007";
            string[] roles = new string[] { "superAdmin", "admin", "user" };

            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    var item = new ApplicationRole { Name = role, Level = role == "superAdmin" ? 1 : role == "admin" ? 2 : 3 };
                    roleManager.Create(item);
                }
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, FirstName = "Samarpratap", LastName = "Singh", Gender = "M", PhoneNumber = "0123456789", ParentId = "NULL", Status = true, EmailConfirmed = true, PhoneNumberConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.AddToRoles(user.Id, roles);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            foreach (var item in Enum.GetValues(typeof(RunNumberStatusType)))
            {
                context.RunNumberStatuses.AddOrUpdate<RunNumberStatus>(
                     new RunNumberStatus() { RunNumberStatusId = Convert.ToByte(item), Status = item.ToString() });
            }

            foreach (var item in Enum.GetValues(typeof(ComponentStatusType)))
            {
                context.ComponentStatuses.AddOrUpdate<ComponentStatus>(
                     new ComponentStatus() { ComponentStatusId = Convert.ToByte(item), Status = item.ToString() });
            }

            foreach (var item in Enum.GetValues(typeof(JobStatusType)))
            {
                context.JobStatuses.AddOrUpdate<JobStatus>(
                     new JobStatus() { JobStatusId = Convert.ToByte(item), Status = item.ToString() });
            }

            foreach (var item in Enum.GetValues(typeof(QueueTypes)))
            {
                context.QueueTypes.AddOrUpdate<QueueType>(
                     new QueueType() { QueueTypeId = Convert.ToByte(item), Status = item.ToString() });
            }

            foreach (var item in Enum.GetValues(typeof(EmailStatusType)))
            {
                context.EmailStatuses.AddOrUpdate<EmailStatus>(
                     new EmailStatus() { EmailStatusId = Convert.ToByte(item), Status = item.ToString() });
            }

            foreach (var item in Enum.GetValues(typeof(EmailKeyword)))
            {
                context.EmailTokens.AddOrUpdate<EmailToken>(
                     new EmailToken() { EmailTokenId = Convert.ToByte(item), Keyword = item.ToString(), Note = EnumHelper.GetEnumDescription((EmailKeyword)item) });
            }

            context.SaveChanges();

        }

    }

    internal class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            base.Generate(addColumnOperation);
        }
    }
}
