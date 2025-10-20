namespace DAL_DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CHITIET_HOADON = new HashSet<CHITIET_HOADON>();
        }

        [Key]
        [StringLength(6)]
        public string IDSP { get; set; }

        [StringLength(100)]
        public string TENSP { get; set; }

        [StringLength(6)]
        public string IDLOAI { get; set; }

        public double? GIABAN { get; set; }

        public double? GIANHAP { get; set; }

        public int? SOLUONG { get; set; }

        [StringLength(2)]
        public string SIZE { get; set; }

        public bool? TRANGTHAI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET_HOADON> CHITIET_HOADON { get; set; }

        public virtual LOAISP LOAISP { get; set; }
    }
}
