using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OGP_Portal.Data.DbModel;

namespace OGP_Portal.Data.DbContext
{
    public class OGP_PortalContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public OGP_PortalContext(DbContextOptions<OGP_PortalContext> options)
            : base(options)
        {
            //Database.Migrate();
        }
        #region Db Set
        #region _A_
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        #endregion

        #region _B_
        public virtual DbSet<B_Partner> B_Partner { get; set; }
        public virtual DbSet<B_User> B_User { get; set; }
        public virtual DbSet<Entry> Entry { get; set; }
        public virtual DbSet<Entry_Log> Entry_Log { get; set; }
        public virtual DbSet<Forcast> Forcast { get; set; }

        #endregion

        #region _C_

        #endregion

        #region _D_
       
        #endregion

        #region _E_
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
       

        #endregion

        #region _F_
        #endregion

        #region _G_
        #endregion

        #region _H_
        #endregion

        #region _I_

        #endregion

        #region _J_
        
        #endregion

        #region _K_
        #endregion

        #region _L_
        #endregion

        #region _M_
        #endregion

        #region _N_
        #endregion

        #region _O_
        #endregion

        #region _P_
        #endregion

        #region _Q_
        #endregion

        #region _R_
        #endregion

        #region _S_

        

        #endregion

        #region _T_
       
        #endregion

        #region _U_
        #endregion

        #region _V_
        #endregion

        #region _W_
        #endregion

        #region _X_
        #endregion

        #region _Y_
        #endregion

        #region _Z_
        #endregion
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //var user = modelBuilder.Entity<ApplicationUser>();

            //user.Property(u => u.UserName)
            //    .IsRequired()
            //    .HasColumnAnnotation("Index", new IndexAnnotation(
            //        new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 1 }));



            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            // Change Default filed datatype & length
            modelBuilder.Entity<ApplicationUser>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(c => c.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserClaim>().Property(x => x.ClaimType).HasMaxLength(50);
            modelBuilder.Entity<UserClaim>().Property(x => x.ClaimValue).HasMaxLength(200);

            modelBuilder.Entity<ApplicationUser>().Property(x => x.Email).HasMaxLength(100);
            modelBuilder.Entity<ApplicationUser>().Property(x => x.UserName).HasMaxLength(100);
            modelBuilder.Entity<ApplicationUser>().Property(x => x.PhoneNumber).HasMaxLength(12);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }

    //public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{
    //    public ApplicationDbContext CreateDbContext(string[] args)
    //    {
    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //           .SetBasePath(Directory.GetCurrentDirectory())
    //           .AddJsonFile("appsettings.json")
    //           .Build();

    //        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        var connectionString = configuration.GetConnectionString("DefaultConnection");

    //        builder.UseSqlServer(connectionString);

    //        return new ApplicationDbContext(builder.Options);
    //    }
    //}
}
