using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Models;

public partial class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DispensingDrug> DispensingDrugs { get; set; }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<DrugProvider> DrugProviders { get; set; }

    public virtual DbSet<DrugType> DrugTypes { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<ManufacturerCountry> ManufacturerCountries { get; set; }

    public virtual DbSet<ReceivingDrug> ReceivingDrugs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=cfif31.ru;database=ISPr24-38_IbragimovaDM_Hospital;uid=ISPr24-38_IbragimovaDM;pwd=ISPr24-38_IbragimovaDM", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DispensingDrug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DrugId, "FK_DispendingDrugs_DrugId_idx");

            entity.HasIndex(e => e.WorkerId, "FK_DispendingDrugs_WorkerId_idx");

            entity.HasOne(d => d.Drug).WithMany(p => p.DispensingDrugs)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("FK_DispendingDrugs_DrugId");

            entity.HasOne(d => d.Worker).WithMany(p => p.DispensingDrugs)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DispendingDrugs_WorkerId");
        });

        modelBuilder.Entity<Drug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DrugProviderId, "FK_Drugs_DrugProviderId_idx");

            entity.HasIndex(e => e.ManufacturerId, "FK_Drugs_ManufacturerId_idx");

            entity.HasIndex(e => e.TypeId, "FK_Drugs_TypeId_idx");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.DrugProvider).WithMany(p => p.Drugs)
                .HasForeignKey(d => d.DrugProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drugs_DrugProviderId");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Drugs)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drugs_ManufacturerId");

            entity.HasOne(d => d.Type).WithMany(p => p.Drugs)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drugs_TypeId");
        });

        modelBuilder.Entity<DrugProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DrugProvider");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<DrugType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DrugType");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("JobTitle");

            entity.HasIndex(e => e.DepartmentId, "FK_JobTitle_DepartmentId_idx");

            entity.Property(e => e.Title)
                .HasMaxLength(60)
                .HasColumnName("TItle");

            entity.HasOne(d => d.Department).WithMany(p => p.JobTitles)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobTitle_DepartmentId");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Manufacturer");

            entity.HasIndex(e => e.CountryId, "FK_Manufacturer_ManufacturerCountryId_idx");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Country).WithMany(p => p.Manufacturers)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manufacturer_ManufacturerCountryId");
        });

        modelBuilder.Entity<ManufacturerCountry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ManufacturerCountry");

            entity.Property(e => e.Country).HasMaxLength(100);
        });

        modelBuilder.Entity<ReceivingDrug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DrugId, "FK_ReceivingDrugs_DrugId_idx");

            entity.HasIndex(e => e.WorkerId, "FK_ReceivingDrugs_WorkerId_idx");

            entity.HasOne(d => d.Drug).WithMany(p => p.ReceivingDrugs)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("FK_ReceivingDrugs_DrugId");

            entity.HasOne(d => d.Worker).WithMany(p => p.ReceivingDrugs)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivingDrugs_WorkerId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Role1)
                .HasMaxLength(45)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.RoleId, "FK_Users_RoleId_idx");

            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(45);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_RoleId");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.JobTitleId, "FK_Workers_JobTitleId_idx");

            entity.HasIndex(e => e.UserId, "FK_Workers_UserId_idx");

            entity.Property(e => e.FullName).HasMaxLength(255);

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Workers)
                .HasForeignKey(d => d.JobTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workers_JobTitleId");

            entity.HasOne(d => d.User).WithMany(p => p.Workers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workers_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
