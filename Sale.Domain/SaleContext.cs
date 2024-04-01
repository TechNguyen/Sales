using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sale.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Domain
{
    public class SaleContext : IdentityDbContext<AppUser, AppRole, Guid>
	{


		private readonly IHttpContextAccessor _httpContextAccessor;
		public SaleContext(DbContextOptions<SaleContext> contexts, IHttpContextAccessor httpContextAccessor) : base(contexts)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			this.SeedRoles(builder);
			builder.Entity<AppUser>().ToTable("AppUser");
			builder.Entity<AppRole>().ToTable("AppRole");

			builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserToken");
			builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRole");
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaim");
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaim");
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogin");
		}

		private void SeedRoles(ModelBuilder builder)
		{
			builder.Entity<AppRole>().HasData(
				new AppRole() { Id = Guid.NewGuid(), Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
				new AppRole() { Id = Guid.NewGuid(), Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
				);
		}

		public DbSet<AppRole> AppRoles { get; set; }



	}
}
