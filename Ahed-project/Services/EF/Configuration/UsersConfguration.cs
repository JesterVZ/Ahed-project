using Ahed_project.Services.EF.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services.EF.Configuration
{
    public class UsersConfguration : IEntityTypeConfiguration<UserEF>
    {
        public void Configure(EntityTypeBuilder<UserEF> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
