﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgresPerfomanceTest.Data;

#nullable disable

namespace PostgresPerfomanceTest.Migrations
{
    [DbContext(typeof(AviaDbContext))]
    partial class AviaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("YearOfFoundation")
                        .HasColumnType("integer");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Flight", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FlightId"));

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlaneId")
                        .HasColumnType("integer");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FlightId");

                    b.HasIndex("PlaneId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Plane", b =>
                {
                    b.Property<int>("PlaneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlaneId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PlaneId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Name");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Flight", b =>
                {
                    b.HasOne("PostgresPerfomanceTest.Data.Plane", "Plane")
                        .WithMany("Flights")
                        .HasForeignKey("PlaneId")
                        .IsRequired();

                    b.Navigation("Plane");
                });

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Plane", b =>
                {
                    b.HasOne("PostgresPerfomanceTest.Data.Company", "Company")
                        .WithMany("Planes")
                        .HasForeignKey("CompanyId")
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Company", b =>
                {
                    b.Navigation("Planes");
                });

            modelBuilder.Entity("PostgresPerfomanceTest.Data.Plane", b =>
                {
                    b.Navigation("Flights");
                });
#pragma warning restore 612, 618
        }
    }
}
