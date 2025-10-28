using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_DoAn
{
        public class TaiKhoanService
        {
        private readonly TiemBanhDB db = new TiemBanhDB();

        // ✅ 1. Lấy toàn bộ tài khoản
        public List<TAIKHOAN> GetAll()
        {
            return db.TAIKHOANs.ToList();
        }

        // ✅ 2. Tìm tài khoản theo ID
        public TAIKHOAN GetById(string id)
        {
            return db.TAIKHOANs.FirstOrDefault(t => t.ID == id);
        }

        // ✅ 3. Kiểm tra đăng nhập
        public TAIKHOAN KiemTraDangNhap(string username, string password)
        {
            return db.TAIKHOANs.FirstOrDefault(t =>
                t.USERNAME == username && t.PASSWORD_USER == password);
        }

        // ✅ 4. Thêm tài khoản
        public bool Add(TAIKHOAN tk)
        {
            if (db.TAIKHOANs.Any(t => t.ID == tk.ID)) return false;

            db.TAIKHOANs.Add(tk);
            db.SaveChanges();
            return true;
        }

        // ✅ 5. Cập nhật tài khoản
        public bool Update(TAIKHOAN tk)
        {
            var existing = db.TAIKHOANs.FirstOrDefault(t => t.ID == tk.ID);
            if (existing == null) return false;

            existing.USERNAME = tk.USERNAME;
            existing.PASSWORD_USER = tk.PASSWORD_USER;
            db.SaveChanges();
            return true;
        }

        // ✅ 6. Xóa tài khoản
        public bool Delete(string id)
        {
            var tk = db.TAIKHOANs.FirstOrDefault(t => t.ID == id);
            if (tk == null) return false;

            db.TAIKHOANs.Remove(tk);
            db.SaveChanges();
            return true;
        }
    }
}
    
