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

namespace GUI_DoAn
{
    public partial class frmQuanLyNhanVien : Form
    {

        public frmQuanLyNhanVien()
        {
            InitializeComponent();
          
        }

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            LoadTaiKhoan();
        }

        private void LoadNhanVien()
        {
            using (var db = new TiemBanhDB())
            {
                var data = db.NHANVIENs.ToList();
                lvNhanVien.Items.Clear();

                foreach (var nv in data)
                {
                    ListViewItem item = new ListViewItem(nv.ID);
                    item.SubItems.Add(nv.TEN_NV);
                    string gioiTinhText = "";

                  
                    var propBool = nv.GetType().GetProperty("GIOITINH") ?? nv.GetType().GetProperty("GioiTinh");
                    if (propBool != null)
                    {
                        var val = propBool.GetValue(nv);
                        if (val is bool b) gioiTinhText = b ? "1" : "0";
                        else if (val is bool?) gioiTinhText = ((bool?)val ?? false) ? "1" : "0";
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

                    item.SubItems.Add(nv.CCCD);
                    item.SubItems.Add(nv.DIACHI_NV);
                    item.SubItems.Add(nv.SDT_NV);
                    item.SubItems.Add(nv.CHUCVU);
                    lvNhanVien.Items.Add(item);
                }

                cmbID.DataSource = data;
                cmbID.DisplayMember = "TEN_NV";
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

        private void btnT_Click(object sender, EventArgs e)
        {
            using (var db = new TiemBanhDB())
            {
                var nv = new NHANVIEN()
                {
                    ID = txtID.Text,
                    TEN_NV = txtHT.Text,
                    GIOITINH = (rdbNam.Checked),
                    CCCD = txtCCCD.Text,
                    DIACHI_NV = txtDC.Text,
                    SDT_NV = txtSDT.Text,
                    CHUCVU = cmbCV.Text
                };

                db.NHANVIENs.Add(nv);
                db.SaveChanges();
                MessageBox.Show("Đã thêm nhân viên mới!");
                LoadNhanVien();
            }
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            using (var db = new TiemBanhDB())
            {
                var nv = db.NHANVIENs.Find(txtID.Text);
                if (nv != null)
                {
                    nv.TEN_NV = txtHT.Text;
                    nv.GIOITINH = (rdbNam.Checked);
                    nv.CCCD = txtCCCD.Text;
                    nv.DIACHI_NV = txtDC.Text;
                    nv.SDT_NV = txtSDT.Text;
                    nv.CHUCVU = cmbCV.Text;

                    db.SaveChanges();
                    MessageBox.Show("Đã cập nhật thông tin nhân viên!");
                    LoadNhanVien();
                }
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            using (var db = new TiemBanhDB())
            {
                var nv = db.NHANVIENs.Find(txtID.Text);
                if (nv != null)
                {
                    db.NHANVIENs.Remove(nv);
                    db.SaveChanges();
                    MessageBox.Show("Đã xóa nhân viên!");
                    LoadNhanVien();
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
            cmbCV.SelectedIndex = -1;
            rdbNam.Checked = false;
            rdbNu.Checked = false;
        }

        private void btnT1_Click(object sender, EventArgs e)
        {
            if (txtMK.Text != txtNLMK.Text)
            {
                MessageBox.Show("Mật khẩu không khớp!");
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
                MessageBox.Show("Đã thêm tài khoản!");
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
            using (var db = new TiemBanhDB())
            {
                var nv = db.NHANVIENs.Find(txtTID.Text);
                lvNhanVien.Items.Clear();

                if (nv != null)
                {
                    ListViewItem item = new ListViewItem(nv.ID);
                    item.SubItems.Add(nv.TEN_NV);
                    string gioiTinhText = "";


                    var propBool = nv.GetType().GetProperty("GIOITINH") ?? nv.GetType().GetProperty("GioiTinh");
                    if (propBool != null)
                    {
                        var val = propBool.GetValue(nv);
                        if (val is bool b) gioiTinhText = b ? "1" : "0";
                        else if (val is bool?) gioiTinhText = ((bool?)val ?? false) ? "1" : "0";
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
                    
                    item.SubItems.Add(nv.CCCD);
                    item.SubItems.Add(nv.DIACHI_NV);
                    item.SubItems.Add(nv.SDT_NV);
                    item.SubItems.Add(nv.CHUCVU);
                    lvNhanVien.Items.Add(item);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên!");
                }
            }
        }

        private void TroVe_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu frm = new frmMenu();
            frm.Show();
        }
    }
}
