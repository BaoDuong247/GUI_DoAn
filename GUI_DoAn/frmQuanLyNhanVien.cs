using BUS_DoAn;
using DAL_DoAn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GUI_DoAn
{
    public partial class frmQuanLyNhanVien : Form
    {
        private readonly NhanVienService nvService = new NhanVienService();
        private readonly TaiKhoanService tkService = new TaiKhoanService();

        private Form parentMenu;
        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }
        public frmQuanLyNhanVien(Form menu)
        {
            InitializeComponent();
            parentMenu = menu; // Lưu tham chiếu đến form menu cha
        }

      

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            SetupListView();
            SetupListViewTaiKhoan();
            LoadNhanVien();
            LoadTaiKhoan();
            ResetInput();
        }

        private void SetupListView()
        {
            lvNhanVien.View = View.Details;
            lvNhanVien.FullRowSelect = true;
            lvNhanVien.GridLines = true;
            lvNhanVien.Columns.Clear();

            lvNhanVien.Columns.Add("Mã NV", 80);
            lvNhanVien.Columns.Add("Tên nhân viên", 150);
            lvNhanVien.Columns.Add("Giới tính", 80);
            lvNhanVien.Columns.Add("CCCD", 120);
            lvNhanVien.Columns.Add("Địa chỉ", 150);
            lvNhanVien.Columns.Add("SĐT", 100);
            lvNhanVien.Columns.Add("Chức vụ", 120);
        }

        private void SetupListViewTaiKhoan()
        {
            lvTaiKhoan.View = View.Details;
            lvTaiKhoan.FullRowSelect = true;
            lvTaiKhoan.GridLines = true;
            lvTaiKhoan.Columns.Clear();

            lvTaiKhoan.Columns.Add("Mã NV", 80);
            lvTaiKhoan.Columns.Add("Tài khoản", 150);
        }

        // ----------------- LOAD DỮ LIỆU -----------------
        private void LoadNhanVien()
        {
            lvNhanVien.Items.Clear();
            var list = nvService.GetAll();

            foreach (var nv in list)
            {
                ListViewItem item = new ListViewItem(nv.ID);
                item.SubItems.Add(nv.TEN_NV);
                item.SubItems.Add((nv.GIOITINH ?? false) ? "Nam" : "Nữ");
                item.SubItems.Add(nv.CCCD);
                item.SubItems.Add(nv.DIACHI_NV);
                item.SubItems.Add(nv.SDT_NV);
                item.SubItems.Add(nv.CHUCVU);
                item.Tag = nv;
                lvNhanVien.Items.Add(item);
            }

            // Load vào combobox để tạo tài khoản
            cmbID.DataSource = list;
            cmbID.DisplayMember = "ID";
            cmbID.ValueMember = "ID";
            cmbID.Enabled = false;
        }

        private void LoadTaiKhoan()
        {
            lvTaiKhoan.Items.Clear();
            var data = tkService.GetAll();

            foreach (var tk in data)
            {
                ListViewItem item = new ListViewItem(tk.ID);
                item.SubItems.Add(tk.USERNAME);
                lvTaiKhoan.Items.Add(item);
            }
        }

        // ----------------- XỬ LÝ NHẬP LIỆU -----------------
        private void ResetInput()
        {
            txtID.Clear();
            txtHT.Clear();
            txtCCCD.Clear();
            txtDC.Clear();
            txtSDT.Clear();
            txtCV.Clear();
            rdbNam.Checked = true;
        }

        private NHANVIEN GetNhanVienFromInput()
        {
            return new NHANVIEN
            {
                ID = txtID.Text.Trim(),
                TEN_NV = txtHT.Text.Trim(),
                GIOITINH = rdbNam.Checked,
                CCCD = txtCCCD.Text.Trim(),
                DIACHI_NV = txtDC.Text.Trim(),
                SDT_NV = txtSDT.Text.Trim(),
                CHUCVU = txtCV.Text.Trim()
            };
        }

        private void btnT_Click(object sender, EventArgs e)
        {

            var nv = GetNhanVienFromInput();

            if (string.IsNullOrEmpty(nv.ID) || string.IsNullOrEmpty(nv.TEN_NV))
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (nvService.GetById(nv.ID) != null)
            {
                MessageBox.Show("⚠️ Mã nhân viên đã tồn tại!");
                return;
            }

            if (nvService.Add(nv))
            {
                MessageBox.Show("✅ Thêm nhân viên thành công!");
                LoadNhanVien();
                ResetInput();
            }
            else MessageBox.Show("❌ Lỗi khi thêm nhân viên!");
        }
        

        private void btnS_Click(object sender, EventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn nhân viên cần sửa trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nv = GetNhanVienFromInput();

            if (string.IsNullOrEmpty(nv.ID))
            {
                MessageBox.Show("⚠️ Mã nhân viên không hợp lệ!");
                return;
            }

            if (nvService.Update(nv))
            {
                MessageBox.Show("✅ Sửa thông tin thành công!");
                LoadNhanVien();
            }
            else
            {
                MessageBox.Show("❌ Lỗi khi sửa thông tin!");
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn nhân viên cần xóa!");
                return;
            }

            var nv = (NHANVIEN)lvNhanVien.SelectedItems[0].Tag;

            if (nv.ID == Form1.currentUserId)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!");
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {nv.TEN_NV} không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (nvService.Delete(nv.ID))
                {
                    tkService.Delete(nv.ID); // Xóa luôn tài khoản nếu có
                    MessageBox.Show("✅ Đã xóa nhân viên và tài khoản!");
                    LoadNhanVien();
                    LoadTaiKhoan();
                }
                else MessageBox.Show("❌ Lỗi khi xóa!");
            }
        }

        private void btnL_Click(object sender, EventArgs e)
        {
            ResetInput();
        }

        private void btnT1_Click(object sender, EventArgs e)
        {
            if (cmbID.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn nhân viên trước khi thêm tài khoản!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = cmbID.SelectedValue.ToString();

            // Kiểm tra nhân viên có tồn tại không
            var nv = nvService.GetById(id);
            if (nv == null)
            {
                MessageBox.Show("❌ Nhân viên chưa tồn tại! Vui lòng thêm thông tin nhân viên trước.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra tài khoản đã có chưa
            var existingTK = tkService.GetById(id);
            if (existingTK != null)
            {
                MessageBox.Show("⚠️ Nhân viên này đã có tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mật khẩu nhập lại
            if (txtMK.Text != txtNLMK.Text)
            {
                MessageBox.Show("❌ Mật khẩu nhập lại không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra dữ liệu rỗng
            if (string.IsNullOrWhiteSpace(txtMNV.Text) || string.IsNullOrWhiteSpace(txtMK.Text))
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo tài khoản
            var tk = new TAIKHOAN
            {
                ID = id,
                USERNAME = txtMNV.Text.Trim(),
                PASSWORD_USER = txtMK.Text.Trim()
            };

            if (tkService.Add(tk))
            {
                MessageBox.Show("✅ Thêm tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan();
                btnL1.PerformClick(); // Reset form
            }
            else
            {
                MessageBox.Show("❌ Lỗi khi thêm tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnS1_Click(object sender, EventArgs e)
        {
            if (cmbID.SelectedIndex == -1 || string.IsNullOrEmpty(cmbID.Text))
            {
                MessageBox.Show("⚠️ Vui lòng chọn nhân viên có tài khoản cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Kiểm tra mật khẩu nhập lại ---
            if (txtMK.Text != txtNLMK.Text)
            {
                MessageBox.Show("❌ Mật khẩu nhập lại không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- Lấy thông tin tài khoản ---
            var tk = new TAIKHOAN
            {
                ID = cmbID.SelectedValue.ToString(),
                USERNAME = txtMNV.Text.Trim(),
                PASSWORD_USER = txtMK.Text.Trim()
            };

            // --- Kiểm tra dữ liệu hợp lệ ---
            if (string.IsNullOrEmpty(tk.USERNAME) || string.IsNullOrEmpty(tk.PASSWORD_USER))
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ thông tin tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Cập nhật ---
            if (tkService.Update(tk))
            {
                MessageBox.Show("✅ Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan();
            }
            else
            {
                MessageBox.Show("❌ Không tìm thấy tài khoản để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnX1_Click(object sender, EventArgs e)
        {
            if (cmbID.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn tài khoản cần xóa!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = cmbID.SelectedValue.ToString();

            var tk = tkService.GetById(id);
            if (tk == null)
            {
                MessageBox.Show("❌ Không tìm thấy tài khoản để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hộp thoại xác nhận Yes/No
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa tài khoản của nhân viên có ID: {id} ({tk.USERNAME}) không?",
                "Xác nhận xóa tài khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            if (tkService.Delete(id))
            {
                MessageBox.Show("✅ Đã xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan();
                btnL1.PerformClick(); // reset form
            }
            else
            {
                MessageBox.Show("❌ Lỗi khi xóa tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnL1_Click(object sender, EventArgs e)
        {
            cmbID.SelectedIndex = -1;
            txtMNV.Clear();
            txtMK.Clear();
            txtNLMK.Clear();
        }

        private void btnTID_Click(object sender, EventArgs e)
        {
            string keyword = txtTID.Text.Trim().ToLower();
            lvNhanVien.Items.Clear();

            var result = nvService.GetAll()
                .Where(n => n.ID.ToLower().Contains(keyword) || n.TEN_NV.ToLower().Contains(keyword))
                .ToList();

            if (result.Count == 0)
            {
                MessageBox.Show("❌ Không tìm thấy nhân viên nào phù hợp!");
                return;
            }

            foreach (var nv in result)
            {
                ListViewItem item = new ListViewItem(nv.ID);
                item.SubItems.Add(nv.TEN_NV);
                item.SubItems.Add((nv.GIOITINH ?? false) ? "Nam" : "Nữ");
                item.SubItems.Add(nv.CCCD);
                item.SubItems.Add(nv.DIACHI_NV);
                item.SubItems.Add(nv.SDT_NV);
                item.SubItems.Add(nv.CHUCVU);
                lvNhanVien.Items.Add(item);
            }
        }

        private void TroVe_Click(object sender, EventArgs e)
        {
            this.Close();          
            parentMenu.Show();
        }

        private void lvNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count == 0) return;
            var nv = (NHANVIEN)lvNhanVien.SelectedItems[0].Tag;

            txtID.Text = nv.ID;
            txtHT.Text = nv.TEN_NV;
            if (nv.GIOITINH.HasValue)
            {
                rdbNam.Checked = nv.GIOITINH.Value;
                rdbNu.Checked = !nv.GIOITINH.Value;
            }
            else
            {
                rdbNam.Checked = false;
                rdbNu.Checked = false;
            }
            txtCCCD.Text = nv.CCCD;
            txtDC.Text = nv.DIACHI_NV;
            txtSDT.Text = nv.SDT_NV;
            txtCV.Text = nv.CHUCVU;
        }

        private void lvTaiKhoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTaiKhoan.SelectedItems.Count == 0) return;

            string id = lvTaiKhoan.SelectedItems[0].SubItems[0].Text;
            var tk = tkService.GetById(id);
            if (tk != null)
            {
                cmbID.SelectedValue = tk.ID;
                txtMNV.Text = tk.USERNAME;
                txtMK.Text = tk.PASSWORD_USER;
                txtNLMK.Text = tk.PASSWORD_USER;
            }
        } 
    }
}
