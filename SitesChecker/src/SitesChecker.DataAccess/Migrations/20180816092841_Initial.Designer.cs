﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SitesChecker.DataAccess;
using System;

namespace SitesChecker.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180816092841_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("SitesChecker.DataAccess.Models.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AvailabilityId");

                    b.Property<string>("Name");

                    b.Property<int>("UpdateDelay");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("AvailabilityId");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("SitesChecker.DataAccess.Models.SiteAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAvailable");

                    b.Property<DateTimeOffset>("LastUpdate");

                    b.Property<int?>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("SiteId")
                        .IsUnique();

                    b.ToTable("SiteAvailabilities");
                });

            modelBuilder.Entity("SitesChecker.DataAccess.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SitesChecker.DataAccess.Models.Site", b =>
                {
                    b.HasOne("SitesChecker.DataAccess.Models.SiteAvailability", "Availability")
                        .WithMany()
                        .HasForeignKey("AvailabilityId");
                });

            modelBuilder.Entity("SitesChecker.DataAccess.Models.SiteAvailability", b =>
                {
                    b.HasOne("SitesChecker.DataAccess.Models.Site", "Site")
                        .WithOne()
                        .HasForeignKey("SitesChecker.DataAccess.Models.SiteAvailability", "SiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
