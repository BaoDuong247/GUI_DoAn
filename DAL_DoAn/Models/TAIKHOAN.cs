namespace DAL_DoAn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAIKHOAN")]
    public partial class TAIKHOAN
    {
        [StringLength(6)]
        public string ID { get; set; }

        [StringLength(50)]
        public string USERNAME { get; set; }

        [StringLength(2048)]
        public string PASSWORD_USER { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
