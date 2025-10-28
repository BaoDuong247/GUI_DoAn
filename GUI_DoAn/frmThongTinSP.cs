using BUS_DoAn;
using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_DoAn
{
    public partial class frmThongTinSP : Form
    {
        private readonly SanPhamService spService = new SanPhamService();
        private readonly HoaDonService hdService = new HoaDonService();
        private readonly ChiTietHoaDonService cthdService = new ChiTietHoaDonService();
        private List<SANPHAM> danhSachSPHoaDon = new List<SANPHAM>();
        TiemBanhDB db = new TiemBanhDB();
        private SANPHAM selectedSP;
        public frmThongTinSP()
        {
            InitializeComponent();
        }


        private NHANVIEN nhanVienHienTai;

        public frmThongTinSP(NHANVIEN nv)
        {
            InitializeComponent();
            nhanVienHienTai = nv; // Lưu thông tin nhân viên hiện tại
        }

        private void btnTVHD_Click(object sender, EventArgs e)
        {
            if (selectedSP == null) return;

            var spTrongHD = danhSachSPHoaDon.FirstOrDefault(sp => sp.IDSP == selectedSP.IDSP);
            if (spTrongHD != null)
            {
                spTrongHD.SOLUONG = (int)nudSL.Value;
            }
            else
            {
                danhSachSPHoaDon.Add(new SANPHAM
                {
                    IDSP = selectedSP.IDSP,
                    TENSP = selectedSP.TENSP,
                    SOLUONG = (int)nudSL.Value,
                    GIABAN = selectedSP.GIABAN
                });
            }

            RefreshDgvHD();
        }

        private void RefreshDgvHD()
        {
            dgvSP.Rows.Clear();
            decimal tongTien = 0;

            foreach (var sp in danhSachSPHoaDon)
            {
                decimal thanhTien = (decimal)(sp.GIABAN ?? 0) * (sp.SOLUONG ?? 0);
                dgvSP.Rows.Add(sp.IDSP, sp.TENSP, sp.SOLUONG, sp.GIABAN, thanhTien);
                tongTien += thanhTien;
            }

            txtTT1.Text = tongTien.ToString("N0");
        }

        private void frmThongTinSP_Load(object sender, EventArgs e)
        {
            LoadLvSP();
            SetupControlsReadOnly();
            timer1.Start();
            txtTTHD.Text = "Chờ tạo...";
            lblTenNhanVien.Text = nhanVienHienTai != null
                                  ? $"Nhân viên: {nhanVienHienTai.TEN_NV}"
                                  : "Nhân viên: [Không xác định]";
        }

        private void LoadLvSP()
        {
            lvSP.Items.Clear();
            var listSP = spService.GetAll(); // Lấy danh sách từ Service

            foreach (var sp in listSP)
            {
                ListViewItem lvi = new ListViewItem(sp.IDSP);
                lvi.SubItems.Add(sp.TENSP);
                lvi.SubItems.Add(sp.LOAISP?.TENLOAI ?? "");
                lvi.SubItems.Add(sp.GIABAN?.ToString("N0"));
                lvi.SubItems.Add(sp.SOLUONG?.ToString());
                lvi.SubItems.Add(sp.SIZE);
                lvi.Tag = sp;
                lvSP.Items.Add(lvi);
            }
        }

        private void SetupControlsReadOnly()
        {
            txtSTT.ReadOnly = true;
            txtSP.ReadOnly = true;
            txtG.ReadOnly = true;
            txtT.ReadOnly = true;
            txtIDSP.ReadOnly = true;
            txtS.ReadOnly = true;
            txtCL.ReadOnly = true;
            txtTT1.ReadOnly = true;
        }

        private void lvSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSP.SelectedItems.Count == 0) return;

            selectedSP = (SANPHAM)lvSP.SelectedItems[0].Tag;

            txtIDSP.Text = selectedSP.IDSP;
            txtSP.Text = selectedSP.TENSP;
            txtG.Text = selectedSP.GIABAN?.ToString();
            txtT.Text = selectedSP.GIABAN?.ToString();
            txtS.Text = selectedSP.SIZE;
            txtCL.Text = selectedSP.SOLUONG?.ToString();
            txtSTT.Text = lvSP.SelectedItems[0].Index.ToString();
            nudSL.Value = 0;
        }

        private void nudSL_ValueChanged(object sender, EventArgs e)
        {
            if (selectedSP == null) return;

            int soLuongTon = selectedSP.SOLUONG ?? 0;
            int soLuongChon = (int)nudSL.Value;

            if (soLuongChon > soLuongTon)
            {
                MessageBox.Show("Số lượng vượt quá tồn kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudSL.Value = soLuongTon;
                soLuongChon = soLuongTon;
            }

            txtCL.Text = (soLuongTon - soLuongChon).ToString();
            txtT.Text = ((selectedSP.GIABAN ?? 0) * soLuongChon).ToString("N0");
        }

        private void btnRSHD_Click(object sender, EventArgs e)
        {
            danhSachSPHoaDon.Clear();
            dgvSP.Rows.Clear();
            txtTT1.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = txtTID.Text.Trim(); // Lấy từ khóa và loại bỏ khoảng trắng thừa

            if (string.IsNullOrEmpty(keyword))
            {
                LoadLvSP();
                return;
            }

            var listSP = db.SANPHAMs.Include("LOAISP")
                        .Where(sp => sp.IDSP.Contains(keyword))
                        .ToList();

            lvSP.Items.Clear(); // Xóa các mục hiện có

            foreach (var sp in listSP)
            {
                ListViewItem lvi = new ListViewItem(sp.IDSP);
                lvi.SubItems.Add(sp.TENSP);
                lvi.SubItems.Add(sp.LOAISP?.TENLOAI ?? "");
                lvi.SubItems.Add(sp.GIABAN?.ToString("N0"));
                lvi.SubItems.Add(sp.SOLUONG?.ToString());
                lvi.SubItems.Add(sp.SIZE); 
                lvi.Tag = sp;
                lvSP.Items.Add(lvi); // Thêm mục vào ListView
            }
        }

        private void frmThongTinSP_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.Dispose(); 
        }

        private void txtKT_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtKT.Text, out decimal tienKhachTra) &&
        decimal.TryParse(txtTT1.Text, out decimal tongTien)) 
            {

                decimal tienThua = tienKhachTra - tongTien;

                if (tienThua < 0)
                {
                    txtTT.Text = "Khách trả thiếu " + Math.Abs(tienThua).ToString("N0") + " đ"; 
                }
                else
                {
                    txtTT.Text = tienThua.ToString("N0");
                }
            }
            else
            {
                txtTT.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("dd/MM/yyyy HH:mm:ss");
            lblThoiGian.Text = formattedTime;
        }

        private void btnXB_Click(object sender, EventArgs e)
        {
            if (dgvSP.Rows.Count == 0 || danhSachSPHoaDon.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào hóa đơn trước khi xem bill.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tenNV = lblTenNhanVien.Text.Replace("Nhân viên: ", "");
            string ngayGio = lblThoiGian.Text;
            string tongTienStr = txtTT1.Text;
            string tienKhachTraStr = txtKT.Text;
            string tienThuaStr = txtTT.Text;

            rtbHD.Clear();

            rtbHD.SelectionFont = new Font("Segoe UI", 16, FontStyle.Bold);
            rtbHD.SelectionAlignment = HorizontalAlignment.Center;
            rtbHD.AppendText("CỬA HÀNG BÁNH TPHCM\n");

            rtbHD.SelectionFont = new Font("Segoe UI", 12, FontStyle.Regular);
            rtbHD.SelectionAlignment = HorizontalAlignment.Center;
            rtbHD.AppendText("Địa chỉ: 123 Đường Bánh, Quận Bánh\n");
            rtbHD.AppendText("SĐT: 0123.456.789\n");
            rtbHD.AppendText("----------------------------------------------------------------------------------------\n");

            rtbHD.SelectionFont = new Font("Segoe UI", 18, FontStyle.Bold);
            rtbHD.SelectionAlignment = HorizontalAlignment.Center;
            rtbHD.AppendText("HÓA ĐƠN THANH TOÁN\n\n");

            rtbHD.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbHD.SelectionAlignment = HorizontalAlignment.Left;
            rtbHD.AppendText($"Nhân viên: {tenNV}\n");
            rtbHD.AppendText($"Ngày giờ: {ngayGio}\n");
            rtbHD.AppendText("----------------------------------------------------------------------------------------\n");

            rtbHD.SelectionFont = new Font("Consolas", 10, FontStyle.Bold);
            rtbHD.AppendText("Mã SP | Tên sản phẩm              | SL | Đ.Giá      | Thành tiền\n");
            rtbHD.AppendText("----------------------------------------------------------------------------------------\n");

            rtbHD.SelectionFont = new Font("Consolas", 10, FontStyle.Regular);
            foreach (DataGridViewRow row in dgvSP.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string idsp = row.Cells[0].Value.ToString();
                    string tenSp = row.Cells[1].Value.ToString();
                    string soLuong = row.Cells[2].Value.ToString();
                    decimal donGia = Convert.ToDecimal(row.Cells[3].Value);
                    decimal thanhTien = Convert.ToDecimal(row.Cells[4].Value);

                    rtbHD.AppendText($"{idsp.PadRight(6)}| {tenSp.PadRight(26)}| {soLuong.PadLeft(3)} | {donGia.ToString("N0").PadLeft(9)} | {thanhTien.ToString("N0").PadLeft(10)}\n");
                }
            }
            rtbHD.AppendText("----------------------------------------------------------------------------------------\n");

            rtbHD.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
            rtbHD.SelectionAlignment = HorizontalAlignment.Right;
            rtbHD.AppendText($"TỔNG TIỀN: {tongTienStr} VNĐ\n");

            rtbHD.SelectionFont = new Font("Segoe UI", 12, FontStyle.Regular);
            rtbHD.AppendText($"Khách trả: {tienKhachTraStr} VNĐ\n");

            if (tienThuaStr.Contains("Khách trả thiếu"))
            {
                rtbHD.SelectionColor = Color.Red;
                rtbHD.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
                rtbHD.AppendText($"{tienThuaStr}\n");
            }
            else
            {
                rtbHD.SelectionColor = Color.Black;
                rtbHD.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
                rtbHD.AppendText($"Tiền thừa: {tienThuaStr} VNĐ\n");
            }

            rtbHD.SelectionColor = Color.Black;
            rtbHD.AppendText("----------------------------------------------------------------------------------------\n");
            rtbHD.SelectionFont = new Font("Segoe UI", 14, FontStyle.Bold);
            rtbHD.SelectionAlignment = HorizontalAlignment.Center;
            rtbHD.AppendText("CẢM ƠN QUÝ KHÁCH! 😊\n");
        }

        private void btnIB_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtTT1.Text.Replace(",", "").Replace(".", ""), out decimal tongTien))
            {
                MessageBox.Show("Tổng tiền không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtKT.Text.Replace(",", "").Replace(".", ""), out decimal tienKhach)
                || tienKhach < tongTien)
            {
                MessageBox.Show("Khách trả chưa đủ tiền.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string newIDHD = TaoIDHoaDonMoi();
                HOADON hd = new HOADON
                {
                    IDHD = newIDHD,
                    NGAYTAO = DateTime.Now,
                    ID_USER = nhanVienHienTai?.ID,
                    TONG_TIEN = (double)tongTien
                };
                hdService.Add(hd);

                foreach (var sp in danhSachSPHoaDon)
                {
                    var spGoc = spService.GetById(sp.IDSP);
                    if (spGoc != null)
                    {
                        CHITIET_HOADON cthd = new CHITIET_HOADON
                        {
                            IDHD = newIDHD,
                            IDSP = sp.IDSP,
                            SL = sp.SOLUONG ?? 0,
                            TONGTIEN = (sp.GIABAN ?? 0) * (sp.SOLUONG ?? 0)
                        };
                        cthdService.Add(cthd);

                        spGoc.SOLUONG -= sp.SOLUONG ?? 0;
                        spService.Update(spGoc);
                    }
                }

                txtTTHD.Text = newIDHD;
                MessageBox.Show($"✅ Lưu hóa đơn {newIDHD} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLvSP();
                btnRSHD_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TaoIDHoaDonMoi()
        {

            var maxId = db.HOADONs
                          .AsEnumerable() 
                          .Where(hd => hd.IDHD != null && hd.IDHD.StartsWith("HD")) // Lọc các ID hợp lệ
                          .Select(hd =>
                          {
                              string numberPart = hd.IDHD.Substring(2);
                              if (int.TryParse(numberPart, out int number))
                              {
                                  return (int?)number; 
                              }
                              return (int?)null; 
                          })
                          .Max(); 

            int nextNumber = (maxId.HasValue ? maxId.Value : 0) + 1;

            return "HD" + nextNumber.ToString(); // ID mới
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Bạn có chắc chắn muốn xóa thông tin hóa đơn hiện tại và làm mới không?",
        "Xác nhận",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    rtbHD.Clear(); 
                    if (txtKT != null)
                        txtKT.Text = "0"; // Reset tiền khách trả

                    MessageBox.Show("Đã làm mới hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi làm mới hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       
    }
}
