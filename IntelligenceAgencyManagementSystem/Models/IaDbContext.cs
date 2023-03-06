using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceAgencyManagementSystem;

public partial class IaDbContext : DbContext
{
    public IaDbContext()
    {
    }

    public IaDbContext(DbContextOptions<IaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WorkersToOp> WorkersToOps { get; set; }

    public virtual DbSet<CoverRole> CoverRoles { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<MilitaryFile> MilitaryFiles { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TasksToWorkers> TasksToWorkers { get; set; }

    public virtual DbSet<WorkingInDepartment> WorkingInDepartments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<WorkersToOp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.WorkerId, "WorkersToOpsWorkerFk");

            entity.HasIndex(e => e.CoverRoleId, "WorkersToOpsCoverRoleFk");

            entity.HasIndex(e => e.OperationId, "WorkersToOpsOperationFk");

            entity.HasOne(d => d.CoverRole).WithMany(p => p.WorkersToOps)
                .HasForeignKey(d => d.CoverRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkersToOpsCoverRoleFk");

            entity.HasOne(d => d.Operation).WithMany(p => p.WorkersToOps)
                .HasForeignKey(d => d.OperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkersToOpsOperationFk");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkersToOps)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkersToOpsAgentFk");
        });

        modelBuilder.Entity<CoverRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.GenderId, "CoverRolesGenderFk");

            entity.Property(e => e.ActivitySummary).HasMaxLength(512);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Legend).HasMaxLength(512);
            entity.Property(e => e.SecondName).HasMaxLength(50);

            entity.HasOne(d => d.Gender).WithMany(p => p.CoverRoles)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("CoverRolesGenderFk");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<MilitaryFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("MilitaryFiles");

            entity.HasIndex(e => e.WorkerId, "MilitaryFilesWorkerFk");

            entity.Property(e => e.FullInformation).HasMaxLength(512);
            entity.Property(e => e.MilitaryRank)
                .HasMaxLength(70)
                .IsFixedLength();

            entity.HasOne(d => d.Worker).WithMany(p => p.MilitaryFiles)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MilitaryFilesWorkerFk");
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DepartmentId, "OperationsDepartmentFk");

            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Department).WithMany(p => p.Operations)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OperationsDepartmentFk");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.GenderId, "WorkersGenderFk");

            entity.Property(e => e.Education).HasMaxLength(512);
            entity.Property(e => e.Experience).HasMaxLength(512);
            entity.Property(e => e.FamilyStatus).HasMaxLength(256);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.HealthInformation).HasMaxLength(512);
            entity.Property(e => e.LegalInformation).HasMaxLength(512);
            entity.Property(e => e.SecondName)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.Gender).WithMany(p => p.Workers)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("WorkersGenderFk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Title)
                .HasMaxLength(70)
                .IsFixedLength();
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.StatusId, "TaskStatusFk");

            entity.HasIndex(e => e.OperationId, "TasksOperationFk");

            entity.Property(e => e.DateStatusSet)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Operation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.OperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TasksOperationFk");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TaskStatusFk");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<TasksToWorkers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.WorkerId, "TasksToWorkersWorkerFk");

            entity.HasIndex(e => e.TaskId, "TasksToWorkersTaskFk");

            entity.Property(e => e.WorkerId).HasColumnName("WorkerId");

            entity.HasOne(d => d.Worker).WithMany(p => p.TasksToWorker)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TasksToWorkersWorkerFk");

            entity.HasOne(d => d.Task).WithMany(p => p.TasksToWorkers)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TasksToWorkersTaskFk");
        });

        modelBuilder.Entity<WorkingInDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("WorkingInDepartment");

            entity.HasIndex(e => e.DepartmentId, "WorkingInDepartmentDepartmentFk");

            entity.HasIndex(e => e.WorkerId, "WorkingInDepartmentWorkerfFk");

            entity.HasIndex(e => e.RoleId, "WorkingInDepartmentRoleFk");

            entity.Property(e => e.Description).HasMaxLength(512);

            entity.HasOne(d => d.Department).WithMany(p => p.WorkingInDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkingInDepartmentDepartmentFk");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkingInDepartments)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkingInDepartmentWorkerFk");

            entity.HasOne(d => d.Role).WithMany(p => p.WorkingInDepartments)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkingInDepartmentRoleFk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
