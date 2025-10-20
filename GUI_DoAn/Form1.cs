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

namespace GUI_DoAn
{
    public partial class Form1 : Form
    {
        TaiKhoanService taiKhoanService = new TaiKhoanService();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMK.UseSystemPasswordChar = true;
        }

        private void ckbHTMK_CheckedChanged(object sender, EventArgs e)
        {
            txtMK.UseSystemPasswordChar = !ckbHTMK.Checked;
        }

        private void btnDN_Click(object sender, EventArgs e)
        {
            try
            {
                string user = txtTK.Text.Trim();
                string pass = txtMK.Text.Trim();

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Tài khoản và Mật khẩu!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isLogin = taiKhoanService.KiemTraDangNhap(user, pass);

                if (isLogin)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmMenu frm = new frmMenu();
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình đăng nhập: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
