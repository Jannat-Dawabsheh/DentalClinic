using DentalClinic.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "Yaqeen",
                    Email = "y@gmail.com",
                    FullName = "Yaqeen Awwad",
                    EmailConfirmed = true

                };
                var user2 = new ApplicationUser
                {
                    UserName = "Jannat",
                    Email = "j@gmail.com",
                    FullName = "jannat Dawabsheh",
                    EmailConfirmed = true

                };
                //var user3 = new Patient
                //{
                //    User = new ApplicationUser
                //    {
                //        UserName = "Mohammad",
                //        Email = "m@gmail.com",
                //        FullName = "Mohammad Awwad",
                //        EmailConfirmed = true
                //    },
                //    Diseases = new List<string> { "Diabetes", "Hypertension" },
                //    Allergies = new List<string> { "Penicillin and antibiotic" },
                //    Gender = "Male",
                //    DateOfBirth = new DateTime(2000, 6, 9)

                //};

                //var user4 = new Doctor
                //{
                //    User = new ApplicationUser
                //    {
                //        UserName = "Zainab",
                //        Email = "z@gmail.com",
                //        FullName = "Zainab Dawabsheh",
                //        EmailConfirmed = true
                //    },
                //    Specialization = "Dental public health specialist",
                //    Gender = "Female",
                //    ExperienceYears = 10
                //};

                await _userManager.CreateAsync(user1, "Pass@1122");
                await _userManager.CreateAsync(user2, "Pass@1122");
                //await _userManager.CreateAsync(user3.User, "Pass@1122");
                //await _userManager.CreateAsync(user4.User, "Pass@1122");

                await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                await _userManager.AddToRoleAsync(user2, "Admin");
                //await _userManager.AddToRoleAsync(user3.User, "Patient");
                //await _userManager.AddToRoleAsync(user4.User, "Doctor");
                
            }


        }
    }
}
