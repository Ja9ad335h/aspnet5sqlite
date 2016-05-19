using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using WhatsappGroups.Data.Contexts;

namespace WhatsappGroups.Data.Migrations
{
    [DbContext(typeof(WhatsappGroupsAdminContext))]
    partial class WhatsappGroupsAdminContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("WhatsappGroups.Data.Models.Audience", b =>
                {
                    b.Property<string>("Id");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "getutcdate()");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LogoutRedirectUri");

                    b.Property<string>("Name");

                    b.Property<string>("RedirectUri");

                    b.Property<string>("Secret");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.RSAPrivateKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "getutcdate()");

                    b.Property<byte[]>("D");

                    b.Property<byte[]>("DP");

                    b.Property<byte[]>("DQ");

                    b.Property<byte[]>("Exponent");

                    b.Property<byte[]>("InverseQ");

                    b.Property<byte[]>("Modulus");

                    b.Property<byte[]>("P");

                    b.Property<byte[]>("Q");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<string>("BusinessName");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "getutcdate()");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("IsActive");

                    b.Property<string>("IsVerified");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<DateTimeOffset?>("UpdateDate");

                    b.Property<string>("UserName");

                    b.Property<string>("Zip");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserInstance", b =>
                {
                    b.Property<string>("UniqueKey");

                    b.Property<string>("Agent");

                    b.Property<string>("HostAddress");

                    b.Property<string>("HostName");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<int?>("UserId");

                    b.HasKey("UniqueKey");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserLogin", b =>
                {
                    b.Property<string>("ProviderKey");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("ProviderKey");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserRole", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("RoleId", "UserId");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.RoleClaim", b =>
                {
                    b.HasOne("WhatsappGroups.Data.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserClaim", b =>
                {
                    b.HasOne("WhatsappGroups.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserInstance", b =>
                {
                    b.HasOne("WhatsappGroups.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserLogin", b =>
                {
                    b.HasOne("WhatsappGroups.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WhatsappGroups.Data.Models.UserRole", b =>
                {
                    b.HasOne("WhatsappGroups.Data.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("WhatsappGroups.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
