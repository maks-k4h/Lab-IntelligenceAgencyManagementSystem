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

    public virtual DbSet<AgentsToOp> AgentsToOps { get; set; }

    public virtual DbSet<CoverRole> CoverRoles { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<MilitaryInformation> MilitaryInformations { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<PersonFile> PersonFiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TasksToPersonFile> TasksToPersonFiles { get; set; }

    public virtual DbSet<WorkingInDepartment> WorkingInDepartments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234567890;database=IA_DB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AgentsToOp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.PersonFileId, "AgentsToOpsAgentFk");

            entity.HasIndex(e => e.CoverRoleId, "AgentsToOpsCoverRoleFk");

            entity.HasIndex(e => e.OperationId, "AgentsToOpsOperationFk");

            entity.HasOne(d => d.CoverRole).WithMany(p => p.AgentsToOps)
                .HasForeignKey(d => d.CoverRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AgentsToOpsCoverRoleFk");

            entity.HasOne(d => d.Operation).WithMany(p => p.AgentsToOps)
                .HasForeignKey(d => d.OperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AgentsToOpsOperationFk");

            entity.HasOne(d => d.PersonFile).WithMany(p => p.AgentsToOps)
                .HasForeignKey(d => d.PersonFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AgentsToOpsAgentFk");
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

        modelBuilder.Entity<MilitaryInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("MilitaryInformation");

            entity.HasIndex(e => e.PersonFileId, "MilitaryInformationPersonFileFk");

            entity.Property(e => e.FullInformation).HasMaxLength(512);
            entity.Property(e => e.MilitaryRank)
                .HasMaxLength(70)
                .IsFixedLength();

            entity.HasOne(d => d.PersonFile).WithMany(p => p.MilitaryInformations)
                .HasForeignKey(d => d.PersonFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MilitaryInformationPersonFileFk");
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

        modelBuilder.Entity<PersonFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.GenderId, "PersonFilesGenderFk");

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

            entity.HasOne(d => d.Gender).WithMany(p => p.PersonFiles)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("PersonFilesGenderFk");
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

        modelBuilder.Entity<TasksToPersonFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.PersonFileId, "TasksToAgencyWorkersAgencyWorkerFk");

            entity.HasIndex(e => e.TaskId, "TasksToAgencyWorkersTaskFk");

            entity.Property(e => e.PersonFileId).HasColumnName("personFileId");

            entity.HasOne(d => d.PersonFile).WithMany(p => p.TasksToPersonFiles)
                .HasForeignKey(d => d.PersonFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TasksToAgencyWorkersAgencyWorkerFk");

            entity.HasOne(d => d.Task).WithMany(p => p.TasksToPersonFiles)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TasksToAgencyWorkersTaskFk");
        });

        modelBuilder.Entity<WorkingInDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("WorkingInDepartment");

            entity.HasIndex(e => e.DepartmentId, "WorkingInDepartmentDepartmentFk");

            entity.HasIndex(e => e.PersonFileId, "WorkingInDepartmentPersonFileFk");

            entity.HasIndex(e => e.RoleId, "WorkingInDepartmentRoleFk");

            entity.Property(e => e.Description).HasMaxLength(512);

            entity.HasOne(d => d.Department).WithMany(p => p.WorkingInDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkingInDepartmentDepartmentFk");

            entity.HasOne(d => d.PersonFile).WithMany(p => p.WorkingInDepartments)
                .HasForeignKey(d => d.PersonFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkingInDepartmentPersonFileFk");

            entity.HasOne(d => d.Role).WithMany(p => p.WorkingInDepartments)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkingInDepartmentRoleFk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
