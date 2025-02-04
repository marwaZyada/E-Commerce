﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class IdentityContext:IdentityDbContext<AppUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options):base(options) 
        {
            
        }
        public DbSet<Address> Addresses { get; set; }
    }
}
