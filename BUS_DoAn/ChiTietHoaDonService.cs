using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_DoAn
{
    public class ChiTietHoaDonService
    {
        private readonly TiemBanhDB db = new TiemBanhDB();

        // Lấy toàn bộ chi tiết hóa đơn
        public List<CHITIET_HOADON> GetAll()
        {
            return db.CHITIET_HOADON.ToList();
        }

        // Lấy chi tiết theo mã hóa đơn
        public List<CHITIET_HOADON> GetByHoaDonId(string idHD)
        {
            if (string.IsNullOrWhiteSpace(idHD))
                return new List<CHITIET_HOADON>();

            return db.CHITIET_HOADON.Where(ct => ct.IDHD == idHD).ToList();
        }

        // Thêm chi tiết hóa đơn mới
        public bool Add(CHITIET_HOADON ct)
        {
            if (ct == null)
                return false;

            // Kiểm tra trùng (IDHD + IDSP) — nếu đã tồn tại thì cập nhật SL thay vì trả false (tùy nhu cầu)
            if (db.CHITIET_HOADON.Any(x => x.IDHD == ct.IDHD && x.IDSP == ct.IDSP))
                return false;

            db.CHITIET_HOADON.Add(ct);
            db.SaveChanges();
            return true;
        }

        // Cập nhật chi tiết hóa đơn
        public bool Update(CHITIET_HOADON ct)
        {
            if (ct == null) return false;

            var existing = db.CHITIET_HOADON
                .FirstOrDefault(x => x.IDHD == ct.IDHD && x.IDSP == ct.IDSP);

            if (existing == null)
                return false;

            existing.SL = ct.SL;
            existing.TONGTIEN = ct.TONGTIEN;

            db.SaveChanges();
            return true;
        }

        // Xóa chi tiết hóa đơn
        public bool Delete(string idHD, string idSP)
        {
            var ct = db.CHITIET_HOADON
                .FirstOrDefault(x => x.IDHD == idHD && x.IDSP == idSP);

            if (ct == null)
                return false;

            db.CHITIET_HOADON.Remove(ct);
            db.SaveChanges();
            return true;
        }

        // Lấy HOADON theo id (helper nội bộ)
        private HOADON GetHoaDonById(string idHD)
        {
            if (string.IsNullOrWhiteSpace(idHD)) return null;
            return db.HOADONs.FirstOrDefault(h => h.IDHD == idHD);
        }

        // Cập nhật tổng tiền của hóa đơn (sử dụng helper ở trên)
        public void UpdateTongTien(string idHD, double tongTien)
        {
            var hd = GetHoaDonById(idHD);
            if (hd != null)
            {
                hd.TONG_TIEN = tongTien;
                db.SaveChanges();
            }
        }

        public bool DeleteByHoaDonId(string idHD)
        {
            if (string.IsNullOrWhiteSpace(idHD))
                return false;

            var listCTHD = db.CHITIET_HOADON.Where(ct => ct.IDHD == idHD).ToList();
            if (!listCTHD.Any())
                return false;

            db.CHITIET_HOADON.RemoveRange(listCTHD);
            db.SaveChanges();
            return true;
        }

    }
}
