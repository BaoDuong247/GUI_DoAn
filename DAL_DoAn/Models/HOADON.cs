namespace DAL_DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADON")]
    public partial class HOADON
    {
        [Key]
        [StringLength(6)]
        public string IDHD { get; set; }

        public DateTime? NGAYTAO { get; set; }

        [StringLength(6)]
        public string ID_USER { get; set; }

        [StringLength(6)]
        public string ID_KH { get; set; }

        public double? TONG_TIEN { get; set; }

        public virtual CHITIET_HOADON CHITIET_HOADON { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
