﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VZTest.Data;

#nullable disable

namespace VZTest.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220607182441_ChangedPasswordHashType")]
    partial class ChangedPasswordHashType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VZTest.Models.Test.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AttemptId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Answers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Answer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Attempt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStarted")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Attempts");
                });

            modelBuilder.Entity("VZTest.Models.Test.CorrectAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId")
                        .IsUnique();

                    b.ToTable("CorrectAnswers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("CorrectAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Correct")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("VZTest.Models.Test.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Balls")
                        .HasColumnType("float");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("VZTest.Models.Test.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxAttempts")
                        .HasColumnType("int");

                    b.Property<bool>("Opened")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("VZTest.Models.UserStar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserStars");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.CheckAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<string>("InternalData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("CheckAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.CheckAnswerOptional", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<string>("InternalData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CheckAnswerOptional_InternalData");

                    b.HasDiscriminator().HasValue("CheckAnswerOptional");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.DateAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<double>("Answer")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("DateAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.DateAnswerOptional", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<DateTime?>("Answer")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateAnswerOptional_Answer");

                    b.HasDiscriminator().HasValue("DateAnswerOptional");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.DoubleAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<double>("Answer")
                        .HasColumnType("float")
                        .HasColumnName("DoubleAnswer_Answer");

                    b.HasDiscriminator().HasValue("DoubleAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.DoubleAnswerOptional", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<double?>("Answer")
                        .HasColumnType("float")
                        .HasColumnName("DoubleAnswerOptional_Answer");

                    b.HasDiscriminator().HasValue("DoubleAnswerOptional");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.IntAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<int>("Answer")
                        .HasColumnType("int")
                        .HasColumnName("IntAnswer_Answer");

                    b.HasDiscriminator().HasValue("IntAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.IntAnswerOptional", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<int?>("Answer")
                        .HasColumnType("int")
                        .HasColumnName("IntAnswerOptional_Answer");

                    b.HasDiscriminator().HasValue("IntAnswerOptional");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.RadioAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<int>("OptionId")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("RadioAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.RadioAnswerOptional", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<int?>("OptionId")
                        .HasColumnType("int")
                        .HasColumnName("RadioAnswerOptional_OptionId");

                    b.HasDiscriminator().HasValue("RadioAnswerOptional");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.TextAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TextAnswer_Answer");

                    b.HasDiscriminator().HasValue("TextAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.Answers.TextAnswerOptional", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.Answer");

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TextAnswerOptional_Answer");

                    b.HasDiscriminator().HasValue("TextAnswerOptional");
                });

            modelBuilder.Entity("VZTest.Models.Test.CorrectAnswers.CorrectDateAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.CorrectAnswer");

                    b.Property<DateTime>("Correct")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("CorrectDateAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.CorrectAnswers.CorrectDoubleAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.CorrectAnswer");

                    b.Property<double>("Correct")
                        .HasColumnType("float")
                        .HasColumnName("CorrectDoubleAnswer_Correct");

                    b.HasDiscriminator().HasValue("CorrectDoubleAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.CorrectAnswers.CorrectIntAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.CorrectAnswer");

                    b.Property<int>("Correct")
                        .HasColumnType("int")
                        .HasColumnName("CorrectIntAnswer_Correct");

                    b.HasDiscriminator().HasValue("CorrectIntAnswer");
                });

            modelBuilder.Entity("VZTest.Models.Test.CorrectAnswers.CorrectTextAnswer", b =>
                {
                    b.HasBaseType("VZTest.Models.Test.CorrectAnswer");

                    b.Property<string>("Correct")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CorrectTextAnswer_Correct");

                    b.HasDiscriminator().HasValue("CorrectTextAnswer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VZTest.Models.Test.CorrectAnswer", b =>
                {
                    b.HasOne("VZTest.Models.Test.Question", null)
                        .WithOne("CorrectAnswer")
                        .HasForeignKey("VZTest.Models.Test.CorrectAnswer", "QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VZTest.Models.Test.Question", b =>
                {
                    b.Navigation("CorrectAnswer");
                });
#pragma warning restore 612, 618
        }
    }
}