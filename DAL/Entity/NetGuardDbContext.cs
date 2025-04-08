using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entity;

public partial class NetGuardDbContext : DbContext
{
    public NetGuardDbContext()
    {
    }

    public NetGuardDbContext(DbContextOptions<NetGuardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUserStatus> TblUserStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-A7SNJDR\\BHIM;Database=NetGuard;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUserStatus>(entity =>
        {
            entity.ToTable("tbl_UserStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeviceName).HasMaxLength(200);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("IPAddress");
            entity.Property(e => e.Latitude).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Location).HasMaxLength(300);
            entity.Property(e => e.Longitude).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
