using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_DoAn
{
    public partial class frmDoanhThuBanHang : Form
    {
        TiemBanhDB db = new TiemBanhDB();
        private Form parentMenu;
        public frmDoanhThuBanHang()
        {
            InitializeComponent();
        }

        public frmDoanhThuBanHang(Form menu)
        {
            InitializeComponent();
            this.parentMenu = menu;
        }

        private void frmDoanhThuBanHang_Load(object sender, EventArgs e)
        {
            LoadDoanhThuToListView(db.HOADONs.ToList());
        }

        private void LoadDoanhThuToListView(List<HOADON> listHD)
        {
            lvDTBH.Items.Clear();
            decimal tongDoanhThu = 0;

            foreach (var hd in listHD.OrderByDescending(h => h.NGAYTAO))
            {
                // Kiểm tra và lấy giá trị THANHTIEN
                decimal thanhTienDecimal = 0;
                if (hd.TONG_TIEN.HasValue)
                {
               
                    thanhTienDecimal = Convert.ToDecimal(hd.TONG_TIEN.Value);
                }

                ListViewItem lvi = new ListViewItem(hd.IDHD);
                string ngayGio = hd.NGAYTAO.HasValue
             ? hd.NGAYTAO.Value.ToString("dd/MM/yyyy HH:mm:ss")
             : "";
                
                lvi.SubItems.Add(ngayGio);
                lvi.SubItems.Add(hd.ID_USER);
                lvi.SubItems.Add(thanhTienDecimal.ToString("N0"));
                lvi.Tag = hd;
                lvDTBH.Items.Add(lvi);

                tongDoanhThu += thanhTienDecimal;
            }

            txtTDT.Text = tongDoanhThu.ToString("N0") + " VNĐ";
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string keyword = txtTKTNTN.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập ngày cần tìm kiếm (ví dụ: 01/01/2023).", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DateTime.TryParseExact(keyword, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime searchDate))
            {
               
                var listHD = db.HOADONs
                               .Where(hd => DbFunctions.TruncateTime(hd.NGAYTAO) == searchDate.Date)
                               .ToList();

                if (listHD.Any())
                {
                    LoadDoanhThuToListView(listHD);
                    MessageBox.Show($"Tìm thấy {listHD.Count} hóa đơn trong ngày {searchDate.ToShortDateString()}.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    LoadDoanhThuToListView(new List<HOADON>()); 
                    txtTDT.Text = "0 VNĐ";
                    MessageBox.Show($"Không tìm thấy hóa đơn nào trong ngày {searchDate.ToShortDateString()}.", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Định dạng ngày không hợp lệ. Vui lòng nhập theo định dạng: dd/MM/yyyy.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            var allHD = db.HOADONs.ToList();

            LoadDoanhThuToListView(allHD);

            txtTKTNTN.Clear();

            MessageBox.Show("Đã tải lại toàn bộ danh sách hóa đơn.", "Reset thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TroVe_Click(object sender, EventArgs e)
        {
            db.Dispose();
            this.Close(); 

            if (parentMenu != null)
            {
                parentMenu.Show();
            }
        }

        private void OpenDoanhThuForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDoanhThuBanHang dtForm = new frmDoanhThuBanHang(this);
            dtForm.Show();
        }
        private void frmDoanhThuBanHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lvDTBH.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedItem = lvDTBH.SelectedItems[0];
            var selectedHoaDon = selectedItem.Tag as HOADON;

            if (selectedHoaDon == null)
            {
                MessageBox.Show("Không thể xác định hóa đơn được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hỏi xác nhận trước khi xóa
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn {selectedHoaDon.IDHD} không?",
                                                  "Xác nhận",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var db = new TiemBanhDB())
                    {
                        // Tìm hóa đơn theo ID
                        var hoaDon = db.HOADONs.Include("CHITIET_HOADON")
                                               .FirstOrDefault(h => h.IDHD == selectedHoaDon.IDHD);

                        if (hoaDon != null)
                        {
                            // Xóa chi tiết hóa đơn trước (vì có ràng buộc khóa ngoại)
                            var chiTietList = db.CHITIET_HOADON.Where(ct => ct.IDHD == hoaDon.IDHD).ToList();
                            db.CHITIET_HOADON.RemoveRange(chiTietList);

                            // Xóa hóa đơn
                            db.HOADONs.Remove(hoaDon);
                            db.SaveChanges();

                            MessageBox.Show("Đã xóa hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Xóa khỏi ListView
                            lvDTBH.Items.Remove(selectedItem);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy hóa đơn trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

