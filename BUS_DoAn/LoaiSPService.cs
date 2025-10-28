using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS_DoAn
{
    public class LoaiSPService
    {
            private readonly TiemBanhDB db = new TiemBanhDB();

            public List<LOAISP> GetAll()
            {
                return db.LOAISPs.ToList();
            }

            public bool Add(LOAISP loai)
            {
                if (string.IsNullOrEmpty(loai.IDLOAI) || string.IsNullOrEmpty(loai.IDLOAI))
                    return false;

                if (db.LOAISPs.Any(x => x.IDLOAI == loai.IDLOAI))
                    return false; 

                db.LOAISPs.Add(loai);
                db.SaveChanges();
                return true;
            }

            public bool Update(LOAISP loai)
            {
                var existing = db.LOAISPs.FirstOrDefault(x => x.IDLOAI == loai.IDLOAI);
                if (existing == null) return false;

                existing.TENLOAI = loai.TENLOAI;
                db.SaveChanges();
                return true;
            }

            public bool Delete(string maLoai)
            {
                var loai = db.LOAISPs.FirstOrDefault(x => x.IDLOAI == maLoai);
                if (loai == null) return false;

                db.LOAISPs.Remove(loai);
                db.SaveChanges();
                return true;
            }

            public LOAISP GetById(string id)
            {
                 using (var db = new TiemBanhDB())
                {
                return db.LOAISPs.FirstOrDefault(l => l.IDLOAI == id);
            }
        }
    }
}
