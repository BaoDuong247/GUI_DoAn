namespace DAL_DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHANVIEN")]
    public partial class NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHANVIEN()
        {
            HOADONs = new HashSet<HOADON>();
        }

        [StringLength(6)]
        public string ID { get; set; }

        [StringLength(50)]
        public string TEN_NV { get; set; }

        public bool? GIOITINH { get; set; }

        [StringLength(13)]
        public string CCCD { get; set; }

        [StringLength(100)]
        public string DIACHI_NV { get; set; }

        [StringLength(10)]
        public string SDT_NV { get; set; }

        [StringLength(20)]
        public string CHUCVU { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}
