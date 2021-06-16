﻿using OGP_Portal.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OGP_Portal
{
    public class OGP_IdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var user = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "",
                MiddleName="",
                UserName = "Admin@admin.com",
                MobileNumber = "8306846936",
                NormalizedUserName = "Admin",
                Email = "Admin@admin.com",
                NormalizedEmail = "Admin@admin.com",
                EmailConfirmed = true,
                IsActive = true,
                IsPasssReset = false,


            };
            if (userManager.FindByEmailAsync(user.UserName).Result != null) return;
            
                var result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            


   
            
        }

        private static void SeedRoles(RoleManager<Role> roleManager)
        {
            #region User Roles
            Dictionary<string, string> normalizedName = new Dictionary<string, string>
            {
                { "Administrator", "Administrator"},     
                { "BusinessPartner", "BusinessPartner"},     
                { "BusinessUser", "BusinessUser"} 
                
            };

            var existrolesList = roleManager.Roles.Select(x => x.Name).ToList();
            if (existrolesList.Any())
            {
                var notExirst = normalizedName.Keys.Except(existrolesList);
                foreach (var notRole in notExirst)
                {
                    string normalized = normalizedName.FirstOrDefault(x => x.Key == notRole).Value;
                    var roleResult = roleManager.CreateAsync(new Role { Name = notRole, NormalizedName = normalized, DisplayRoleName = normalized }).Result;
                }
            }
            else
            {
                foreach (var objRole in normalizedName.Keys)
                {
                    string normalized = normalizedName.FirstOrDefault(x => x.Key == objRole).Value;
                    IdentityResult roleResult = roleManager.CreateAsync(new Role { Name = objRole, NormalizedName = normalized, DisplayRoleName = normalized }).Result;
                }
            }
            #endregion
        }

        public static long GetTimeZoneId(string currentTimeZone, string connection)
        {
            using (var sqlCon = new SqlConnection(connection))
            {
                var query = $@"EXEC GetCurrentTimeZone '{currentTimeZone}';";
                sqlCon.Open();
                var sqlCmd = new SqlCommand(query, sqlCon);
                var result = sqlCmd.ExecuteScalar();
                sqlCon.Close();
                return (long?)result ?? 0;
            }
        }
    }
}
