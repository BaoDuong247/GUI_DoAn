using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_DoAn
{
    public class HoaDonService
    {
        private readonly TiemBanhDB db = new TiemBanhDB();

        // Lấy tất cả hóa đơn
        public List<HOADON> GetAll()
        {
            return db.HOADONs.ToList();
        }

        // Lấy hóa đơn theo ID
        public HOADON GetById(string id)
        {
            return db.HOADONs.FirstOrDefault(hd => hd.IDHD == id);
        }

        // Thêm hóa đơn mới
        public bool Add(HOADON hd)
        {
            if (hd == null || db.HOADONs.Any(x => x.IDHD == hd.IDHD))
                return false;

            db.HOADONs.Add(hd);
            db.SaveChanges();
            return true;
        }

        // Sửa hóa đơn
        public bool Update(HOADON hd)
        {
            var existing = db.HOADONs.FirstOrDefault(x => x.IDHD == hd.IDHD);
            if (existing == null)
                return false;

            existing.NGAYTAO = hd.NGAYTAO;
            existing.ID_USER = hd.ID_USER;
            existing.TONG_TIEN = hd.TONG_TIEN;

            db.SaveChanges();
            return true;
        }

        // Xóa hóa đơn
        public bool Delete(string id)
        {
            var hd = db.HOADONs.FirstOrDefault(x => x.IDHD == id);
            if (hd == null)
                return false;

            // Xóa luôn chi tiết hóa đơn liên quan
            var cthdList = db.CHITIET_HOADON.Where(x => x.IDHD == id).ToList();
            db.CHITIET_HOADON.RemoveRange(cthdList);

            db.HOADONs.Remove(hd);
            db.SaveChanges();
            return true;
        }
            public void UpdateSoLuong(string idSP, int soLuongTru)
        {
            var sp = db.SANPHAMs.FirstOrDefault(x => x.IDSP == idSP);
            if (sp != null)
            {
                sp.SOLUONG = (sp.SOLUONG ?? 0) - soLuongTru;
                if (sp.SOLUONG < 0) sp.SOLUONG = 0;
                db.SaveChanges();
            }
        }

        public List<HOADON> GetByDate(DateTime date)
        {
            // Lấy tất cả hóa đơn có ngày tạo trùng với ngày truyền vào
            return db.HOADONs
                     .Where(hd => DbFunctions.TruncateTime(hd.NGAYTAO) == date.Date)
                     .ToList();
        }
    }
}
