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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOADON()
        {
            CHITIET_HOADON = new HashSet<CHITIET_HOADON>();
        }

        [Key]
        [StringLength(6)]
        public string IDHD { get; set; }

        public DateTime? NGAYTAO { get; set; }

        [StringLength(6)]
        public string ID_USER { get; set; }

        public double? TONG_TIEN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIET_HOADON> CHITIET_HOADON { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
