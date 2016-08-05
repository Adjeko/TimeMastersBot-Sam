using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TimeMasters.Web.Data;

namespace TimeMasters.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Environment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FxProfile");

                    b.Property<bool>("IsDebugging");

                    b.Property<int>("LogID");

                    b.Property<string>("MachineName");

                    b.Property<string>("SessionId");

                    b.HasKey("ID");

                    b.HasIndex("LogID")
                        .IsUnique();

                    b.ToTable("Environment");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Events", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Level");

                    b.Property<int>("LogID");

                    b.Property<string>("Logger");

                    b.Property<string>("Message");

                    b.Property<int>("SequenceID");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("ID");

                    b.HasIndex("LogID")
                        .IsUnique();

                    b.ToTable("Events");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Exception", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<int>("EventsID");

                    b.Property<string>("HResult");

                    b.Property<string>("HelpLint");

                    b.Property<string>("InnerException");

                    b.Property<string>("Message");

                    b.Property<string>("Source");

                    b.Property<string>("StackTrace");

                    b.Property<string>("TargetSite");

                    b.HasKey("ID");

                    b.HasIndex("EventsID")
                        .IsUnique();

                    b.ToTable("Exception");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.ExceptionWrapper", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AsString");

                    b.Property<int>("EventsID");

                    b.Property<int>("Hresult");

                    b.Property<string>("TypeName");

                    b.HasKey("ID");

                    b.HasIndex("EventsID")
                        .IsUnique();

                    b.ToTable("ExceptionWrapper");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Log", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.MetroLogVersion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Build");

                    b.Property<int>("EnvironmentID");

                    b.Property<int>("Major");

                    b.Property<int>("MajorRevision");

                    b.Property<int>("Minor");

                    b.Property<int>("MinorRevision");

                    b.Property<int>("Revision");

                    b.HasKey("ID");

                    b.HasIndex("EnvironmentID")
                        .IsUnique();

                    b.ToTable("MetroLogVersion");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TimeMasters.Web.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Environment", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.Logging.Log", "Log")
                        .WithOne("Environment")
                        .HasForeignKey("TimeMasters.Web.Models.Logging.Environment", "LogID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Events", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.Logging.Log", "Log")
                        .WithOne("Events")
                        .HasForeignKey("TimeMasters.Web.Models.Logging.Events", "LogID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.Exception", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.Logging.Events", "Events")
                        .WithOne("Exception")
                        .HasForeignKey("TimeMasters.Web.Models.Logging.Exception", "EventsID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.ExceptionWrapper", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.Logging.Events", "Events")
                        .WithOne("ExceptionWrapper")
                        .HasForeignKey("TimeMasters.Web.Models.Logging.ExceptionWrapper", "EventsID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeMasters.Web.Models.Logging.MetroLogVersion", b =>
                {
                    b.HasOne("TimeMasters.Web.Models.Logging.Environment", "Environment")
                        .WithOne("MetroLogVersion")
                        .HasForeignKey("TimeMasters.Web.Models.Logging.MetroLogVersion", "EnvironmentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
