using BUS_DoAn;
using DAL_DoAn.Models;
using System;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GUI_DoAn
{
    public partial class frmQuanLySanPham : Form
    {
        private readonly SanPhamService spService = new SanPhamService();
        private readonly LoaiSPService loaiService = new LoaiSPService();
        TiemBanhDB db = new TiemBanhDB();
        private byte[] currentImageBytes;
        private string tenFileAnh;
        public frmQuanLySanPham()
        {
            InitializeComponent();
        }
        private void LoadSanPham()
        {
            lvQLSP.Items.Clear();
            var list = spService.GetAll();

            foreach (var sp in list)
            {
                ListViewItem item = new ListViewItem(sp.IDSP);
                item.SubItems.Add(sp.TENSP);
                item.SubItems.Add(sp.LOAISP?.TENLOAI ?? "");
                item.SubItems.Add(sp.GIABAN?.ToString("N0"));
                item.SubItems.Add(sp.GIANHAP?.ToString("N0"));
                item.SubItems.Add(sp.SOLUONG?.ToString());
                item.SubItems.Add(sp.SIZE);
                item.SubItems.Add(sp.SOLUONG > 0 ? "Còn hàng" : "Hết hàng");
                item.SubItems.Add(sp.ANHSP != null ? "Đã có ảnh" : "(Chưa có ảnh)");
                item.Tag = sp;
                lvQLSP.Items.Add(item);
            }
        }

        private void LoadLoaiSP()
        {
            lvTTSP.Items.Clear();
            var list = loaiService.GetAll();

            foreach (var loai in list)
            {
                ListViewItem item = new ListViewItem(loai.IDLOAI);
                item.SubItems.Add(loai.TENLOAI);
                item.Tag = loai;
                lvTTSP.Items.Add(item);
            }

            cmbLoai.DataSource = list;
            cmbLoai.DisplayMember = "TENLOAI";
            cmbLoai.ValueMember = "IDLOAI";
        }

        // ==================== HÀM HỖ TRỢ ====================

        private void ResetSanPham()
        {
            txtID.Clear();
            txtSP.Clear();
            cmbLoai.SelectedIndex = -1;
            txtGB.Text = "0";
            txtGN.Text = "0";
            txtSL.Text = "0";
            cmbS.SelectedIndex = -1;
            rdbC.Checked = true;
            picAvatar.Image = null;
            currentImageBytes = null;
        }

        private void ResetLoaiSP()
        {
            txtML.Clear();
            txtTL.Clear();
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            if (imageIn == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }

        private void SetupListViewSanPham()
        {
            lvQLSP.View = View.Details;
            lvQLSP.FullRowSelect = true;
            lvQLSP.GridLines = true;
            lvQLSP.Columns.Clear();

            lvQLSP.Columns.Add("Mã SP", 90);
            lvQLSP.Columns.Add("Tên SP", 180);
            lvQLSP.Columns.Add("Loại", 120);
            lvQLSP.Columns.Add("Giá bán", 100);
            lvQLSP.Columns.Add("Giá nhập", 100);
            lvQLSP.Columns.Add("Số lượng", 80);
            lvQLSP.Columns.Add("Size", 60);
            lvQLSP.Columns.Add("Trạng thái", 100);
            lvQLSP.Columns.Add("Ảnh", 120);
        }

        private void SetupListViewLoaiSP()
        {
            lvTTSP.View = View.Details;
            lvTTSP.FullRowSelect = true;
            lvTTSP.GridLines = true;
            lvTTSP.Columns.Clear();

            lvTTSP.Columns.Add("Mã loại", 120);
            lvTTSP.Columns.Add("Tên loại", 220);
        }
        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                SetupListViewSanPham();
                SetupListViewLoaiSP();
                LoadLoaiSP();
                LoadSanPham();
                ResetSanPham();
                ResetLoaiSP();

                cmbS.Items.Clear();
                cmbS.Items.Add("N"); // Nhỏ
                cmbS.Items.Add("V"); // Vừa
                cmbS.Items.Add("L"); // Lớn
                cmbS.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvQLSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvQLSP.SelectedItems.Count == 0) return;

            var sp = (SANPHAM)lvQLSP.SelectedItems[0].Tag;
            if (sp == null) return;

            txtID.Text = sp.IDSP;
            txtSP.Text = sp.TENSP;
            cmbLoai.SelectedValue = sp.IDLOAI;
            txtGB.Text = sp.GIABAN?.ToString();
            txtGN.Text = sp.GIANHAP?.ToString();
            txtSL.Text = sp.SOLUONG?.ToString();
            cmbS.SelectedItem = sp.SIZE;
            rdbC.Checked = sp.SOLUONG > 0;
            rdbH.Checked = sp.SOLUONG <= 0;
            picAvatar.Image = ByteArrayToImage(sp.ANHSP);
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            ResetSanPham();
            txtID.ReadOnly = false;
            SetEditModeSanPham(true); // Cho phép nhập dữ liệu mới
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            if (lvQLSP.SelectedItems.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng chọn sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Khóa ID (không được phép sửa)
            txtID.ReadOnly = true;

            // Cho phép chỉnh sửa các thông tin còn lại
            SetEditModeSanPham(true);

            // Thông báo cho người dùng biết đang ở chế độ chỉnh sửa
            MessageBox.Show("Bạn có thể chỉnh sửa thông tin sản phẩm và nhấn 'Lưu' để cập nhật!",
                            "Chế độ sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (lvQLSP.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var sp = (SANPHAM)lvQLSP.SelectedItems[0].Tag;
                    spService.Delete(sp.IDSP);
                    LoadSanPham();
                    ResetSanPham();
                    MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadLvSanPham()
        {
            try
            {
                lvQLSP.Items.Clear();

                // Lấy toàn bộ danh sách sản phẩm từ DB, kèm theo thông tin loại
                var listSanPham = db.SANPHAMs
                    .Include(sp => sp.LOAISP)
                    .OrderBy(sp => sp.IDSP)
                    .ToList();

                foreach (var sp in listSanPham)
                {
                    ListViewItem item = new ListViewItem(sp.IDSP);
                    item.SubItems.Add(sp.TENSP);
                    item.SubItems.Add(sp.LOAISP?.TENLOAI ?? "(Chưa có loại)");
                    item.SubItems.Add(sp.GIABAN?.ToString("N0") ?? "0");
                    item.SubItems.Add(sp.GIANHAP?.ToString("N0") ?? "0");
                    item.SubItems.Add(sp.SOLUONG?.ToString() ?? "0");
                    item.SubItems.Add(sp.SIZE ?? "N/A");

                    // Trạng thái hiển thị “Còn hàng” hoặc “Hết hàng”
                    string trangThai = (sp.SOLUONG.GetValueOrDefault(0) > 0 && sp.TRANGTHAI == true)
                        ? "Còn hàng"
                        : "Hết hàng";
                    item.SubItems.Add(trangThai);

                    // Ảnh minh họa
                    string moTaAnh = (sp.ANHSP != null && sp.ANHSP.Length > 0)
                        ? "Đã có ảnh"
                        : "(Chưa có ảnh)";
                    item.SubItems.Add(moTaAnh);

                    // Lưu đối tượng gốc để dễ xử lý khi chọn
                    item.Tag = sp;

                    lvQLSP.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetEditModeSanPham(bool enable)
        {
            // Cho phép hoặc khóa các control khi thêm/sửa sản phẩm
            txtID.ReadOnly = !enable;
            txtSP.ReadOnly = !enable;
            cmbLoai.Enabled = enable;
            txtGB.ReadOnly = !enable;
            txtGN.ReadOnly = !enable;
            txtSL.ReadOnly = !enable;
            cmbS.Enabled = enable;
            rdbC.Enabled = enable;
            rdbH.Enabled = enable;
            btnCA.Enabled = enable;   // Nút chọn ảnh
            btnL.Enabled = enable;    // Nút lưu

            // Khi bật chế độ chỉnh sửa => ẩn các nút T/S/X để tránh thao tác chồng chéo
            btnT.Enabled = !enable;
            btnS.Enabled = !enable;
            btnX.Enabled = !enable;
        }
        private void btnL_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtID.Text) ||
                    string.IsNullOrWhiteSpace(txtSP.Text) ||
                    cmbLoai.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ ID, Tên SP và chọn Loại SP.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tìm sản phẩm
                SANPHAM sp = db.SANPHAMs.Find(txtID.Text);
                bool isNew = (sp == null);

                if (isNew)
                {
                    sp = new SANPHAM();
                    sp.IDSP = txtID.Text;
                    db.SANPHAMs.Add(sp);
                }

                // Gán giá trị mới
                sp.TENSP = txtSP.Text;
                sp.IDLOAI = cmbLoai.SelectedValue.ToString();
                sp.GIABAN = double.TryParse(txtGB.Text, out double gb) ? gb : 0;
                sp.GIANHAP = double.TryParse(txtGN.Text, out double gn) ? gn : 0;
                sp.SOLUONG = int.TryParse(txtSL.Text, out int sl) ? sl : 0;
                sp.SIZE = cmbS.SelectedItem?.ToString();
                sp.TRANGTHAI = rdbC.Checked;

                // Ảnh
                if (picAvatar.Image != null)
                    sp.ANHSP = ImageToByteArray(picAvatar.Image);
                else
                    sp.ANHSP = null;

                // Lưu thay đổi
                db.SaveChanges();

                // Làm mới ListView
                LoadLvSanPham();

                SetEditModeSanPham(false);

                MessageBox.Show(isNew ? "Thêm sản phẩm thành công!" : "Cập nhật sản phẩm thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCA_Click(object sender, EventArgs e)
        {
        
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
            
                picAvatar.Image = new Bitmap(ofd.FileName);

                byte[] imageBytes = File.ReadAllBytes(ofd.FileName);

                currentImageBytes = imageBytes;

                tenFileAnh = Path.GetFileName(ofd.FileName);
            }
        
        }

        private void txtSL_TextChanged(object sender, EventArgs e)
        {

            if (txtSL.ReadOnly == false)
            {
                if (int.TryParse(txtSL.Text, out int soLuong))
                {
                    if (soLuong <= 0)
                    {
                        rdbH.Checked = true;
                    }
                    else
                    {
                        rdbC.Checked = true;
                    }
                }
            }
        }

        private void rdbH_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbH.Enabled && rdbH.Checked)
            {
                txtSL.Text = "0"; 
            }
        }

        private void rdbC_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbC.Enabled && rdbC.Checked && txtSL.Text == "0")
            {
                txtSL.Text = "1"; 
            }
        }

        private void lvTTSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTTSP.SelectedItems.Count == 0) return;
            var loai = (LOAISP)lvTTSP.SelectedItems[0].Tag;
            txtML.Text = loai.IDLOAI;
            txtTL.Text = loai.TENLOAI;
        }

        private void btnT1_Click(object sender, EventArgs e)
        {
            ResetLoaiSP();
            txtML.ReadOnly = false;
        }

        private void btnS1_Click(object sender, EventArgs e)
        {
            if (lvTTSP.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txtML.ReadOnly = true;
        }

        private void btnX1_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Kiểm tra xem có chọn sản phẩm nào không
                if (lvQLSP.SelectedItems.Count == 0)
                {
                    MessageBox.Show("⚠️ Vui lòng chọn thông tin sản phẩm cần xóa!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Lấy sản phẩm được chọn
                var sp = (SANPHAM)lvQLSP.SelectedItems[0].Tag;

                // ✅ Hỏi xác nhận trước khi xóa
                var confirm = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm '{sp.TENSP}' không?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    // Gọi Service để xóa
                    if (spService.Delete(sp.IDLOAI))
                    {
                        MessageBox.Show("✅ Xóa sản phẩm thành công!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSanPham(); // Tải lại danh sách
                    }
                    else
                    {
                        MessageBox.Show("❌ Lỗi khi xóa sản phẩm!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnL1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtML.Text) || string.IsNullOrWhiteSpace(txtTL.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LOAISP loai = new LOAISP
                {
                    IDLOAI = txtML.Text,
                    TENLOAI = txtTL.Text
                };

                bool isNew = loaiService.GetById(loai.IDLOAI) == null;

                if (isNew)
                    loaiService.Add(loai);
                else
                    loaiService.Update(loai);

                LoadLoaiSP();
                LoadSanPham();
                MessageBox.Show(isNew ? "Thêm loại thành công!" : "Cập nhật loại thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu loại SP: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmQuanLySanPham_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.Dispose();
        }

     
        private void btnTID_Click(object sender, EventArgs e)
        {
            string keyword = txtTID.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadSanPham();
                MessageBox.Show("Vui lòng nhập mã sản phẩm để tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var list = spService.GetAll().Where(sp => sp.IDSP.Contains(keyword)).ToList();
            lvQLSP.Items.Clear();

            if (list.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm có mã: " + keyword, "Thông báo");
                return;
            }

            foreach (var sp in list)
            {
                ListViewItem item = new ListViewItem(sp.IDSP);
                item.SubItems.Add(sp.TENSP);
                item.SubItems.Add(sp.LOAISP?.TENLOAI ?? "");
                item.SubItems.Add(sp.GIABAN?.ToString("N0"));
                item.SubItems.Add(sp.GIANHAP?.ToString("N0"));
                item.SubItems.Add(sp.SOLUONG?.ToString());
                item.SubItems.Add(sp.SIZE);
                item.SubItems.Add(sp.SOLUONG > 0 ? "Còn hàng" : "Hết hàng");
                item.SubItems.Add(sp.ANHSP != null ? "Đã có ảnh" : "(Chưa có ảnh)");
                lvQLSP.Items.Add(item);
            }
        }

        private void txtTID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnTID_Click(sender, e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu frm = new frmMenu();
            frm.Show();
        }
    }
}
