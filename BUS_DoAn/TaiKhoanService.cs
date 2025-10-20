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

        public bool KiemTraDangNhap(string username, string password)
        {
            var tk = db.TAIKHOANs
                .FirstOrDefault(x => x.USERNAME == username && x.PASSWORD_USER == password);

            return tk != null;
        }
    }
}
