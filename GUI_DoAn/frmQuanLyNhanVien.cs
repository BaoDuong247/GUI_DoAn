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

        private Form parentMenu;
        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }
        public frmQuanLyNhanVien(Form menu)
        {
            InitializeComponent();
            parentMenu = menu; 
        }

      

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            LoadTaiKhoan();
            KiemTraThongTinTaiKhoan();
            cmbID.SelectedIndexChanged += (s, ev) => KiemTraThongTinTaiKhoan();
            txtMNV.TextChanged += (s, ev) => KiemTraThongTinTaiKhoan();
            txtMK.TextChanged += (s, ev) => KiemTraThongTinTaiKhoan();
            txtNLMK.TextChanged += (s, ev) => KiemTraThongTinTaiKhoan();
        }



        private void LoadNhanVien()
        {
            using (var db = new TiemBanhDB())
            {
                var data = db.NHANVIENs.ToList();
                lvNhanVien.View = View.Details;
                lvNhanVien.FullRowSelect = true;
                lvNhanVien.GridLines = true;
                lvNhanVien.Columns.Clear();
                lvNhanVien.Columns.Add("ID", 70);
                lvNhanVien.Columns.Add("Họ Tên", 120);
                lvNhanVien.Columns.Add("Giới Tính", 70);
                lvNhanVien.Columns.Add("CCCD", 100);
                lvNhanVien.Columns.Add("Địa Chỉ", 120);
                lvNhanVien.Columns.Add("SĐT", 90);
                lvNhanVien.Columns.Add("Chức Vụ", 80);
                lvNhanVien.Items.Clear();

                foreach (var nv in data)
                {
                    string gioiTinhText = "";

                    var propBool = nv.GetType().GetProperty("GIOITINH") ?? nv.GetType().GetProperty("GioiTinh");
                    if (propBool != null)
                    {
                        var val = propBool.GetValue(nv);
                        if (val is bool b) gioiTinhText = b ? "Nam" : "Nữ";
                        else if (val is bool?) gioiTinhText = ((bool?)val ?? false) ? "Nam" : "Nữ";
                    }
                    else
                    {
                        var propStr = nv.GetType().GetProperty("GioiTinh") ?? nv.GetType().GetProperty("GIOITINH");
                        if (propStr != null)
                        {
                            var sval = propStr.GetValue(nv) as string;
                            gioiTinhText = string.IsNullOrEmpty(sval) ? "" : sval;
                        }
                    }

                    ListViewItem item = new ListViewItem(nv.ID);
                    item.SubItems.Add(nv.TEN_NV);
                    item.SubItems.Add(gioiTinhText);
                    item.SubItems.Add(nv.CCCD);
                    item.SubItems.Add(nv.DIACHI_NV);
                    item.SubItems.Add(nv.SDT_NV);
                    item.SubItems.Add(nv.CHUCVU);
                    lvNhanVien.Items.Add(item);
                }

                cmbID.DataSource = data;
                cmbID.DisplayMember = "ID";
                cmbID.ValueMember = "ID";
            }
        }

        private void LoadTaiKhoan()
        {
            using (var db = new TiemBanhDB())
            {
                var data = db.TAIKHOANs.ToList();
                lvTaiKhoan.Items.Clear();

                foreach (var tk in data)
                {
                    ListViewItem item = new ListViewItem(tk.ID);
                    item.SubItems.Add(tk.USERNAME);
                    lvTaiKhoan.Items.Add(item);
                }
            }
        }

        private bool KiemTraHopLe()
        {
            string id = txtID.Text.Trim();
            string ten = txtHT.Text.Trim();
            string cccd = txtCCCD.Text.Trim();
            string diachi = txtDC.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string chucvu = txtCV.Text.Trim();

            if (string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(ten) ||
                string.IsNullOrWhiteSpace(cccd) ||
                string.IsNullOrWhiteSpace(diachi) ||
                string.IsNullOrWhiteSpace(sdt) ||
                string.IsNullOrWhiteSpace(chucvu) ||
                (!rdbNam.Checked && !rdbNu.Checked))
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ tất cả thông tin!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(id, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("⚠️ ID chỉ được chứa chữ và số, không chứa ký tự đặc biệt!", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (id.Length != 6)
            {
                MessageBox.Show("⚠️ Mã nhân viên phải gồm đúng 6 ký tự!", "Sai độ dài", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("⚠️ Vui lòng nhập mã nhân viên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(cccd, @"^\d{12,13}$"))
            {
                MessageBox.Show("❌ CCCD phải là số và có 12 hoặc 13 chữ số!", "Sai định dạng CCCD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^0\d{9}$"))
            {
                MessageBox.Show("❌ Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số!", "Sai định dạng SĐT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private void KiemTraThongTinTaiKhoan()
        {
            bool hopLe = true;
            string id = (cmbID.SelectedValue != null) ? cmbID.SelectedValue.ToString() : "";
            string username = txtMNV.Text.Trim();
            string pass = txtMK.Text.Trim();
            string rePass = txtNLMK.Text.Trim();

            if (string.IsNullOrEmpty(id))
                hopLe = false;

            if (string.IsNullOrWhiteSpace(username))
                hopLe = false;

            if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6)
                hopLe = false;

            if (string.IsNullOrWhiteSpace(rePass) || rePass != pass)
                hopLe = false;

            btnT1.Enabled = hopLe;
            btnS1.Enabled = hopLe;
            btnX1.Enabled = hopLe;
            btnL1.Enabled = hopLe;
        }

        private void btnT_Click(object sender, EventArgs e)
        {

            if (!KiemTraHopLe())
                return;

            using (var db = new TiemBanhDB())
            {
                string id = txtID.Text.Trim();

                if (db.NHANVIENs.Any(n => n.ID == id))
                {
                    MessageBox.Show("❗ID nhân viên đã tồn tại! Vui lòng nhập ID khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nv = new NHANVIEN()
                {
                    ID = id,
                    TEN_NV = txtHT.Text.Trim(),
                    GIOITINH = rdbNam.Checked,
                    CCCD = txtCCCD.Text.Trim(),
                    DIACHI_NV = txtDC.Text.Trim(),
                    SDT_NV = txtSDT.Text.Trim(),
                    CHUCVU = txtCV.Text.Trim()
                };

                db.NHANVIENs.Add(nv);
                db.SaveChanges();
                MessageBox.Show("✅ Đã thêm nhân viên mới thành công!");
                LoadNhanVien();
            }
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("⚠️ Vui lòng chọn hoặc nhập ID nhân viên cần sửa!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHT.Text) ||
                string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                string.IsNullOrWhiteSpace(txtDC.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtCV.Text) ||
                (!rdbNam.Checked && !rdbNu.Checked))
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ thông tin nhân viên trước khi sửa!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtCCCD.Text, @"^\d{12}$"))
            {
                MessageBox.Show("❌ Số CCCD không hợp lệ! CCCD phải có 12 chữ số.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("❌ Số điện thoại không hợp lệ! SDT phải gồm 10 chữ số và bắt đầu bằng 0.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var db = new TiemBanhDB())
            {
                var nv = db.NHANVIENs.Find(txtID.Text.Trim());
                if (nv == null)
                {
                    MessageBox.Show("❗Không tìm thấy nhân viên để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                nv.TEN_NV = txtHT.Text.Trim();
                nv.GIOITINH = rdbNam.Checked;
                nv.CCCD = txtCCCD.Text.Trim();
                nv.DIACHI_NV = txtDC.Text.Trim();
                nv.SDT_NV = txtSDT.Text.Trim();
                nv.CHUCVU = txtCV.Text.Trim();

                db.SaveChanges();
                MessageBox.Show("✅ Đã cập nhật thông tin nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVien();
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            string id = txtID.Text.Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập ID nhân viên cần xóa!");
                return;
            }

            string currentUserId = Form1.currentUserId;
            if (id == currentUserId)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new TiemBanhDB())
            {
                try
                {
                    var nv = db.NHANVIENs.FirstOrDefault(n => n.ID == id);
                    if (nv == null)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên cần xóa!");
                        return;
                    }

                    if (MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {nv.TEN_NV} và tài khoản liên quan không?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    var tk = db.TAIKHOANs.FirstOrDefault(t => t.ID == id);
                    if (tk != null)
                        db.TAIKHOANs.Remove(tk);

                    db.NHANVIENs.Remove(nv);
                    db.SaveChanges();

                    MessageBox.Show("Đã xóa nhân viên và tài khoản thành công!");
                    LoadNhanVien();
                    LoadTaiKhoan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}");
                }
            }
        }

        private void btnL_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtHT.Clear();
            txtCCCD.Clear();
            txtDC.Clear();
            txtSDT.Clear();
            txtCV.Clear();
            rdbNam.Checked = false;
            rdbNu.Checked = false;
        }

        private void btnT1_Click(object sender, EventArgs e)
        {
            if (!btnT1.Enabled)
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ và đúng định dạng trước khi thêm tài khoản!",
                                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMK.Text != txtNLMK.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            using (var db = new TiemBanhDB())
            {
                var tk = new TAIKHOAN()
                {
                    ID = cmbID.SelectedValue.ToString(),
                    USERNAME = txtMNV.Text,
                    PASSWORD_USER = txtMK.Text
                };

                db.TAIKHOANs.Add(tk);
                db.SaveChanges();
                MessageBox.Show("✅ Đã thêm tài khoản thành công!");
                LoadTaiKhoan();
            }
        }

        private void btnS1_Click(object sender, EventArgs e)
        {
            using (var db = new TiemBanhDB())
            {
                var id = cmbID.SelectedValue.ToString();
                var tk = db.TAIKHOANs.Find(id);
                if (tk != null)
                {
                    tk.USERNAME = txtMNV.Text;
                    tk.PASSWORD_USER = txtMK.Text;
                    db.SaveChanges();
                    MessageBox.Show("Đã cập nhật tài khoản!");
                    LoadTaiKhoan();
                }
            }
        }

        private void btnX1_Click(object sender, EventArgs e)
        {
            using (var db = new TiemBanhDB())
            {
                var id = cmbID.SelectedValue.ToString();
                var tk = db.TAIKHOANs.Find(id);
                if (tk != null)
                {
                    db.TAIKHOANs.Remove(tk);
                    db.SaveChanges();
                    MessageBox.Show("Đã xóa tài khoản!");
                    LoadTaiKhoan();
                }
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

            using (var db = new TiemBanhDB())
            {
                lvNhanVien.Items.Clear();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadNhanVien();
                    return;
                }

                var result = db.NHANVIENs
                    .Where(n => n.ID.ToLower().Contains(keyword) || n.TEN_NV.ToLower().Contains(keyword))
                    .ToList();

                if (result.Count == 0)
                {
                    MessageBox.Show("❌ Không tìm thấy nhân viên nào phù hợp!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var nv in result)
                {
                    string gioiTinhText = "";
                    var propBool = nv.GetType().GetProperty("GIOITINH") ?? nv.GetType().GetProperty("GioiTinh");
                    if (propBool != null)
                    {
                        var val = propBool.GetValue(nv);
                        if (val is bool b) gioiTinhText = b ? "Nam" : "Nữ";
                        else if (val is bool?) gioiTinhText = ((bool?)val ?? false) ? "Nam" : "Nữ";
                    }
                    else
                    {
                        var propStr = nv.GetType().GetProperty("GioiTinh") ?? nv.GetType().GetProperty("GIOITINH");
                        if (propStr != null)
                        {
                            var sval = propStr.GetValue(nv) as string;
                            gioiTinhText = string.IsNullOrEmpty(sval) ? "" : sval;
                        }
                    }

                    ListViewItem item = new ListViewItem(nv.ID);
                    item.SubItems.Add(nv.TEN_NV);
                    item.SubItems.Add(gioiTinhText);
                    item.SubItems.Add(nv.CCCD);
                    item.SubItems.Add(nv.DIACHI_NV);
                    item.SubItems.Add(nv.SDT_NV);
                    item.SubItems.Add(nv.CHUCVU);
                    lvNhanVien.Items.Add(item);
                }
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

            ListViewItem item = lvNhanVien.SelectedItems[0];

            txtID.Text = item.SubItems[0].Text;
            txtHT.Text = item.SubItems[1].Text;
            string gioiTinh = item.SubItems[2].Text;
            if (gioiTinh == "Nam" || gioiTinh == "1")
            {
                rdbNam.Checked = true;
                rdbNu.Checked = false;
            }
            else if (gioiTinh == "Nữ" || gioiTinh == "0")
            {
                rdbNu.Checked = true;
                rdbNam.Checked = false;
            }
            else
            {
                rdbNam.Checked = false;
                rdbNu.Checked = false;
            }
            txtCCCD.Text = item.SubItems[3].Text;
            txtDC.Text = item.SubItems[4].Text;
            txtSDT.Text = item.SubItems[5].Text;
            txtCV.Text = item.SubItems[6].Text;
        }

        private void lvTaiKhoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTaiKhoan.SelectedItems.Count == 0) return;
            ListViewItem item = lvTaiKhoan.SelectedItems[0];
            string id = item.SubItems[0].Text;
            string username = item.SubItems[1].Text;
            cmbID.SelectedValue = id;
            txtMNV.Text = username;
            using (var db = new TiemBanhDB())
            {
                var tk = db.TAIKHOANs.Find(id);
                if (tk != null)
                {
                    txtMK.Text = tk.PASSWORD_USER;
                    txtNLMK.Text = tk.PASSWORD_USER;
                }
            }
        }
        
    }
}
