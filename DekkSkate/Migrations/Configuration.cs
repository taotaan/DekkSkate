namespace DekkSkate.Migrations
{
    using DekkSkate.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DekkSkate.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DekkSkate.Models.ApplicationDbContext";
        }

        protected override void Seed(DekkSkate.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // createRoleAdmin
            if (!roleManager.RoleExists("Admin"))
            {
                var roleResult = roleManager.Create(new IdentityRole("Admin"));

                if (roleResult.Succeeded)
                {
                    Console.WriteLine("Role: 'Admin' create success!");
                }
                else
                {
                    Console.WriteLine("Fail to create role 'Admin'");
                }
            }
            else
            {
                Console.WriteLine("Role 'Admin' is already exixts");
            }

            //CreateRoleService

            if (!roleManager.RoleExists("Service"))
            {
                var roleResult = roleManager.Create(new IdentityRole("Service"));

                if (roleResult.Succeeded)
                {
                    Console.WriteLine("Role: 'Service' create success!");
                }
                else
                {
                    Console.WriteLine("Fail to create role 'Service'");
                }
            }
            else
            {
                Console.WriteLine("Role 'Service' is already exixts");
            }

            //CreateRoleUser
            if (!roleManager.RoleExists("User"))
            {
                var roleResult = roleManager.Create(new IdentityRole("User"));

                if (roleResult.Succeeded)
                {
                    Console.WriteLine("Role: 'User' create success!");
                }
                else
                {
                    Console.WriteLine("Fail to create role 'User'");
                }
            }
            else
            {
                Console.WriteLine("Role 'User' is already exixts");
            }

            var user = userManager.FindByEmail("Admin@a.com");

            if (user != null)
            {
                var currentRole = userManager.GetRoles(user.Id);


                if (currentRole.Any())
                {
                    userManager.RemoveFromRole(user.Id, currentRole.ToString());

                }

                userManager.AddToRole(user.Id, "Admin");

            }



        }
    }
}
