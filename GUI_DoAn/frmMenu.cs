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
    public partial class frmMenu : Form
    {
        private readonly NhanVienService nvService = new NhanVienService();
        private readonly SanPhamService spService = new SanPhamService();
        private readonly HoaDonService hdService = new HoaDonService();
        private NHANVIEN nhanVienDangNhap; 

        public frmMenu(NHANVIEN nv)
        {
            InitializeComponent();
            nhanVienDangNhap = nv; // Lưu thông tin nhân viên đăng nhập
        }
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
            if (Form1.currentUserRole != "Admin")
            {
                btnTV.Visible = false;     
                btnTV.Text = "🔒";
                btnSP.Visible = false;
                btnSP.Text = "🔒";
                btnDT.Visible = false;
                btnDT.Text = "🔒";
            }
            else
            {
                btnTV.Enabled = true;
                btnSP.Enabled = true;
                btnDT.Enabled = true;
            }
        }

        private void btnTV_Click(object sender, EventArgs e)
        {
            frmQuanLyNhanVien f = new frmQuanLyNhanVien(this);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnDX_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
          
                this.Hide();

                Form1 f = new Form1();
                f.Show();

                this.Close();
            }
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Bạn có chắc chắn muốn thoát không?",
        "Xác nhận thoát",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
          );

            if (result == DialogResult.Yes)
            {
                Application.Exit(); //Thoát toàn bộ chương trình
            }

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            int soLuongNV = nvService.GetAll().Count;
            int soLuongSP = spService.GetAll().Count;

            if (richTextBox1.Text == "" && richTextBox2.Text == "")
            {
                richTextBox1.ReadOnly = true;
                richTextBox1.BackColor = Color.White;
                richTextBox1.Font = new Font("Segoe UI", 28, FontStyle.Bold);
                richTextBox1.ForeColor = Color.DarkGreen;
                richTextBox1.Text = soLuongSP.ToString();
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

                richTextBox2.ReadOnly = true;
                richTextBox2.BackColor = Color.White;
                richTextBox2.Font = new Font("Segoe UI", 28, FontStyle.Bold);
                richTextBox2.ForeColor = Color.DarkBlue;
                richTextBox2.Text = soLuongNV.ToString();
                richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            }
            else
            {
                richTextBox1.Clear();
                richTextBox2.Clear();
            }
        }
        

        

        private void btnSP_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmQuanLySanPham f = new frmQuanLySanPham(); 

            f.FormClosed += (s, args) =>
            {
                this.Show();
            };

            f.ShowDialog(); 
        }

     

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e); // Gọi phương thức gốc để đảm bảo các hành động đóng form mặc định được thực hiện
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!Application.OpenForms.OfType<Form1>().Any()) 
                {
                    Application.Exit();
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            frmThongTinSP f = new frmThongTinSP(nhanVienDangNhap);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void frmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnDT_Click(object sender, EventArgs e)
        {
            frmDoanhThuBanHang f = new frmDoanhThuBanHang();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
    

