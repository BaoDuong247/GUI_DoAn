using BUS_DoAn;
using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_DoAn
{
    public partial class frmDoanhThuBanHang : Form
    {
        private readonly HoaDonService hdService = new HoaDonService();
        private readonly ChiTietHoaDonService cthdService = new ChiTietHoaDonService();
        private Form parentMenu;
        TiemBanhDB db = new TiemBanhDB();
        public frmDoanhThuBanHang()
        {
            InitializeComponent();
        }

        public frmDoanhThuBanHang(Form menu)
        {
            InitializeComponent();
            this.parentMenu = menu; // Lưu tham chiếu đến form menu cha
        }

        private void frmDoanhThuBanHang_Load(object sender, EventArgs e)
        {
            LoadDoanhThuToListView(hdService.GetAll());
        }

        private void LoadDoanhThuToListView(List<HOADON> listHD)
        {
            lvDTBH.Items.Clear();
            decimal tongDoanhThu = 0;

            foreach (var hd in listHD.OrderByDescending(h => h.NGAYTAO))
            {
                decimal thanhTien = (decimal)(hd.TONG_TIEN ?? 0);
                ListViewItem item = new ListViewItem(hd.IDHD);
                item.SubItems.Add(hd.NGAYTAO?.ToString("dd/MM/yyyy HH:mm:ss") ?? "");
                item.SubItems.Add(hd.ID_USER ?? "");
                item.SubItems.Add(thanhTien.ToString("N0") + " VNĐ");
                item.Tag = hd;
                lvDTBH.Items.Add(item);
                tongDoanhThu += thanhTien;
            }

            txtTDT.Text = tongDoanhThu.ToString("N0") + " VNĐ";
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string input = txtTKTNTN.Text.Trim();

            if (DateTime.TryParseExact(input, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                LoadDoanhThuToListView(hdService.GetByDate(date));
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng ngày (dd/MM/yyyy)",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadDoanhThuToListView(hdService.GetAll());
            txtTKTNTN.Clear();
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
                MessageBox.Show("Vui lòng chọn hóa đơn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hd = lvDTBH.SelectedItems[0].Tag as HOADON;
            if (hd == null) return;

            var confirm = MessageBox.Show($"Xóa hóa đơn {hd.IDHD}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                cthdService.DeleteByHoaDonId(hd.IDHD);
                hdService.Delete(hd.IDHD);
                LoadDoanhThuToListView(hdService.GetAll());
                MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

