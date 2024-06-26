﻿using GamingRental.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace GamingRental.Data.Seeding
{
    public class Seeder
    {
        private readonly GamingRentalDbContext _context;

        public Seeder(GamingRentalDbContext context)
        {
            this._context = context;
        }

        public void SeedData()
        {
            this.SeedUsers();
            this.SeedConsoleTypes();
            this.SeedRentalAgreements();
        }

        public void SeedConsoleTypes()
        {
            if (!this._context.ConsoleTypes.Any())
            {
                var consoleTypes = new List<ConsoleType>()
                {
                    new ConsoleType
                    {
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        Id = Guid.Parse("83f333c7-a36b-47b2-a1e6-30a3f8cb237e"),
                        Description = "PLAYSTATION 5"
                    },
                    new ConsoleType
                    {
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        Id = Guid.Parse("63979d6c-089c-4929-a295-dfd4b7bcb312"),
                        Description = "PLAYSTATION 5"
                    },
                    new ConsoleType
                    {
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        Id = Guid.Parse("e4545687-14a0-4369-a679-f278f4736008"),
                        Description = "PLAYSTATION 5"
                    },
                    new ConsoleType
                    {
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        Id = Guid.Parse("a96f13dc-36c0-426e-8e48-ef85ce96cf52"),
                        Description = "Xbox One "
                    }
                };

                this._context.ConsoleTypes.AddRange(consoleTypes);
            }

            this._context.SaveChanges();
        }

        private void SeedRentalAgreements()
        {
            if (!this._context.RentalAgreements.Any())
            {
                foreach (var user in this._context.Users)
                {
                    var rentalAgreement = new RentalAgreement
                    {
                        ConsoleTypeId = Guid.Parse("83f333c7-a36b-47b2-a1e6-30a3f8cb237e"),
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        Id = Guid.NewGuid(),
                        RentalEndDate = DateTime.Now,
                        RentalStartDate = DateTime.Now,
                        UserId = user.Id,
                    };

                    this._context.RentalAgreements.Add(rentalAgreement);
                }

              this._context.SaveChanges();
            }
        }

        private void SeedUsers()
        {
            if (!this._context.Users.Any())
            {
                var adminRole = new IdentityRole(Role.Administrator)
                {
                    Id = "d8c9aa16-5dff-442f-a653-2108c4990f63",
                    NormalizedName = Role.Administrator.Normalize(NormalizationForm.FormC)
                };
                var clientRole = new IdentityRole(Role.Client)
                {
                    Id = "0bf31124-8c8d-4693-bd6b-22bfbd7dd7a1",
                    NormalizedName = Role.Client.Normalize(NormalizationForm.FormC)
                };
                this._context.Roles.AddRange(
                    new List<IdentityRole>
                    {
                        adminRole,
                        clientRole,
                    }
                );

                this.SeedUser(GetDummyUser("admin", "adminadmin"), adminRole);
                this.SeedUser(GetDummyUser("dani", "danidani"), clientRole);
                this._context.SaveChanges();
            }
        }

        private AddUserIdentity GetDummyUser(string name, string password)
        {
            var email = $"{name}@gmail.com";
            var id = Guid.NewGuid();
            var user = new AddUserIdentity
            {
                Id = id.ToString(),
                Email = email,
                UserName = email,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var hasher = new PasswordHasher<AddUserIdentity>();
            user.PasswordHash = hasher.HashPassword(user, password);

            return user;
        }

        private void SeedUser(AddUserIdentity user, params IdentityRole[] roles)
        {
            this._context.Users.Add(user);
            foreach (var role in roles)
            {
                this._context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
            }
        }
    }
}
