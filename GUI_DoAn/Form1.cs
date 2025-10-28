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
        public static string currentUserId = ""; //Lưu ID
        public static string currentUserRole = ""; //Lưu Chức Vụ
        private readonly TaiKhoanService taiKhoanService = new TaiKhoanService();
        private NhanVienService nvService = new NhanVienService();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMK.UseSystemPasswordChar = true; // Ẩn mật khẩu khi tải form

        }

        private void ckbHTMK_CheckedChanged(object sender, EventArgs e)
        {
            txtMK.UseSystemPasswordChar = !ckbHTMK.Checked; // Hiện/Ẩn mật khẩu
        }

        private void btnDN_Click(object sender, EventArgs e)
        {
            string username = txtTK.Text.Trim();
            string password = txtMK.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!");
                return;
            }

            var tk = taiKhoanService.KiemTraDangNhap(username, password);

            if (tk != null)
            {
                // Lấy thông tin nhân viên tương ứng
                var nv = nvService.GetById(tk.ID);

                if (nv != null)
                {
                    currentUserId = nv.ID;
                    currentUserRole = nv.CHUCVU;
                }

                MessageBox.Show("✅ Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                frmMenu menu = new frmMenu(nv);
                menu.Show();
            }
            else
            {
                MessageBox.Show("❌ Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        private void Form1_Activated(object sender, EventArgs e)
        {
            txtTK.Clear(); 
            txtMK.Clear();
            txtTK.Focus(); 
        }
    }
}
