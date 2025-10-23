namespace DAL_DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CHITIET_HOADON
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string IDHD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string IDSP { get; set; }

        public int? SL { get; set; }

        public double? TONGTIEN { get; set; }

        public virtual HOADON HOADON { get; set; }

        public virtual SANPHAM SANPHAM { get; set; }
    }
}
