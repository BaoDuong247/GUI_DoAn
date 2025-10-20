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
    public partial class frmMenu : Form
    {
        public frmMenu(TAIKHOAN taiKhoan)
        {
            InitializeComponent();
        }
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnTV_Click(object sender, EventArgs e)
        {
            frmQuanLyNhanVien f = new frmQuanLyNhanVien();
            this.Hide();            // Ẩn form Menu
            f.ShowDialog();         // Mở form Quản Lý Nhân Viên
            this.Show();
        }

        private void btnDX_Click(object sender, EventArgs e)
        {
            this.Hide();            // Ẩn form Menu
            Form1 f = new Form1();  // Tạo lại form đăng nhập
            f.ShowDialog();         // Mở lại form đăng nhập
            this.Close();
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
