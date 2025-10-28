using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_DoAn
{
    public class SanPhamService
    {
        private readonly TiemBanhDB db = new TiemBanhDB();

       
        public List<SANPHAM> GetAll()
        {
            return db.SANPHAMs.Include("LOAISP").ToList();
        }

        public bool Add(SANPHAM sp)
        {
            if (string.IsNullOrEmpty(sp.IDSP) || string.IsNullOrEmpty(sp.TENSP))
                return false;

            if (db.SANPHAMs.Any(x => x.IDSP == sp.IDSP))
                return false; // trùng mã SP

            db.SANPHAMs.Add(sp);
            db.SaveChanges();
            return true;
        }

        public bool Update(SANPHAM sp)
        {
            var existing = db.SANPHAMs.FirstOrDefault(x => x.IDSP == sp.IDSP);
            if (existing == null) return false;

            existing.TENSP = sp.TENSP;
            existing.IDLOAI = sp.IDLOAI;
            existing.GIABAN = sp.GIABAN;
            existing.SOLUONG = sp.SOLUONG;
            existing.SIZE = sp.SIZE;
            existing.TRANGTHAI = sp.TRANGTHAI;

            db.SaveChanges();
            return true;
        }

        public bool Delete(string idsp)
        {
            var sp = db.SANPHAMs.FirstOrDefault(x => x.IDSP == idsp);
            if (sp == null) return false;

            db.SANPHAMs.Remove(sp);
            db.SaveChanges();
            return true;
        }

        public List<SANPHAM> Search(string keyword)
        {
            return db.SANPHAMs
                .Where(x => x.TENSP.Contains(keyword) || x.IDSP.Contains(keyword))
                .ToList();
        }

        public SANPHAM GetById(string id)
        {
            using (var db = new TiemBanhDB())
            {
                return db.SANPHAMs.FirstOrDefault(sp => sp.IDSP == id);
            }
        }
    }
}

