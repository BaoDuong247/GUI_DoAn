using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL_DoAn.Models
{
    public partial class TiemBanhDB : DbContext
    {
        public TiemBanhDB()
            : base("name=TiemBanhDB2")
        {
        }

        public virtual DbSet<CHITIET_HOADON> CHITIET_HOADON { get; set; }
        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<LOAISP> LOAISPs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CHITIET_HOADON>()
                .Property(e => e.IDHD)
                .IsUnicode(false);

            modelBuilder.Entity<CHITIET_HOADON>()
                .Property(e => e.IDSP)
                .IsUnicode(false);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.IDHD)
                .IsUnicode(false);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.ID_USER)
                .IsUnicode(false);

            modelBuilder.Entity<LOAISP>()
                .Property(e => e.IDLOAI)
                .IsUnicode(false);

            modelBuilder.Entity<LOAISP>()
                .HasMany(e => e.SANPHAMs)
                .WithOptional(e => e.LOAISP)
                .WillCascadeOnDelete();

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.CCCD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.SDT_NV)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .HasMany(e => e.HOADONs)
                .WithOptional(e => e.NHANVIEN)
                .HasForeignKey(e => e.ID_USER)
                .WillCascadeOnDelete();

            modelBuilder.Entity<NHANVIEN>()
                .HasOptional(e => e.TAIKHOAN)
                .WithRequired(e => e.NHANVIEN)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.IDSP)
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.IDLOAI)
                .IsUnicode(false);

            modelBuilder.Entity<TAIKHOAN>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<TAIKHOAN>()
                .Property(e => e.USERNAME)
                .IsUnicode(false);
        }
    }
}
