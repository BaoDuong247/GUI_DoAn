using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_DoAn
{
    public class NhanVienService
    {
        private readonly TiemBanhDB db = new TiemBanhDB();

        // 🔹 Lấy toàn bộ danh sách nhân viên
        public List<NHANVIEN> GetAll()
        {
            return db.NHANVIENs.ToList();
        }

        // 🔹 Lấy nhân viên theo ID
        public NHANVIEN GetById(string id)
        {
            return db.NHANVIENs.FirstOrDefault(nv => nv.ID == id);
        }

        // 🔹 Thêm nhân viên mới
        public bool Add(NHANVIEN nv)
        {
            if (nv == null)
                throw new ArgumentNullException(nameof(nv));

            if (db.NHANVIENs.Any(x => x.ID == nv.ID))
                throw new Exception($"Nhân viên với ID '{nv.ID}' đã tồn tại!");

            db.NHANVIENs.Add(nv);
            return db.SaveChanges() > 0;
        }

        // 🔹 Cập nhật nhân viên
        public bool Update(NHANVIEN nv)
        {
            if (nv == null)
                throw new ArgumentNullException(nameof(nv));

            var existing = db.NHANVIENs.FirstOrDefault(x => x.ID == nv.ID);
            if (existing == null)
                throw new Exception("Không tìm thấy nhân viên để cập nhật!");

            existing.TEN_NV = nv.TEN_NV;
            existing.GIOITINH = nv.GIOITINH;
            existing.CCCD = nv.CCCD;
            existing.DIACHI_NV = nv.DIACHI_NV;
            existing.SDT_NV = nv.SDT_NV;
            existing.CHUCVU = nv.CHUCVU;

            db.Entry(existing).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        // 🔹 Xóa nhân viên theo ID
        public bool Delete(string id)
        {
            var nv = db.NHANVIENs.FirstOrDefault(x => x.ID == id);
            if (nv == null)
                throw new Exception("Không tìm thấy nhân viên cần xóa!");

            db.NHANVIENs.Remove(nv);
            return db.SaveChanges() > 0;
        }

        // 🔹 Tìm kiếm theo tên hoặc số điện thoại
        public List<NHANVIEN> Search(string keyword)
        {
            keyword = keyword?.Trim().ToLower() ?? "";
            return db.NHANVIENs
                     .Where(x => x.TEN_NV.ToLower().Contains(keyword)
                              || x.SDT_NV.Contains(keyword))
                     .ToList();
        }

        // 🔹 Kiểm tra nhân viên có tồn tại không
        public bool Exists(string id)
        {
            return db.NHANVIENs.Any(x => x.ID == id);
        }

        // 🔹 Lấy danh sách nhân viên theo chức vụ (ví dụ: “Admin” hoặc “Nhân viên”)
        public List<NHANVIEN> GetByRole(string role)
        {
            return db.NHANVIENs
                     .Where(x => x.CHUCVU == role)
                     .ToList();
        }

        // 🔹 Đếm số lượng nhân viên
        public int Count()
        {
            return db.NHANVIENs.Count();
        }


    }
}

