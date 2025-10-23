using DAL_DoAn.Models;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;

namespace GUI_DoAn
{
    public partial class frmQuanLySanPham : Form
    {
        TiemBanhDB db = new TiemBanhDB();
        private byte[] currentImageBytes;
        private string tenFileAnh;
        public frmQuanLySanPham()
        {
            InitializeComponent();
        }
        private void LoadLvSanPham()
        {
            lvQLSP.Items.Clear();
            var listSanPham = db.SANPHAMs.Include(sp => sp.LOAISP).ToList();

            foreach (var sp in listSanPham)
            {
                ListViewItem lvi = new ListViewItem(sp.IDSP);
                lvi.SubItems.Add(sp.TENSP);
                lvi.SubItems.Add(sp.LOAISP?.TENLOAI ?? "");
                lvi.SubItems.Add(sp.GIABAN?.ToString("N0"));
                lvi.SubItems.Add(sp.GIANHAP?.ToString("N0"));
                lvi.SubItems.Add(sp.SOLUONG?.ToString());
                lvi.SubItems.Add(sp.SIZE);

                string trangThai = (sp.SOLUONG.GetValueOrDefault(0) > 0) ? "Còn hàng" : "Hết hàng";
                lvi.SubItems.Add(trangThai);

                string moTaAnh = (sp.ANHSP != null && sp.ANHSP.Length > 0)
                    ? "Đã có ảnh minh họa"
                    : "(Chưa có ảnh minh họa)";
                lvi.SubItems.Add(moTaAnh);
                lvi.Tag = sp;

                lvQLSP.Items.Add(lvi);
            }
        }

        private void LoadLvLoaiSanPham()
        {
            lvTTSP.Items.Clear();
            var listLoaiSP = db.LOAISPs.ToList();

            foreach (var loai in listLoaiSP)
            {
                ListViewItem lvi = new ListViewItem(loai.IDLOAI);

                lvi.SubItems.Add(loai.TENLOAI);
                lvi.Tag = loai; 
                lvTTSP.Items.Add(lvi);
            }
        }

        private void LoadCmbLoai()
        {
            var listLoaiSP = db.LOAISPs.ToList();
            cmbLoai.DataSource = listLoaiSP;
            cmbLoai.DisplayMember = "IDLOAI";
            cmbLoai.ValueMember = "IDLOAI";
        }

        private void ClearFormSanPham()
        {
            txtID.Text = "";
            txtSP.Text = "";
            cmbLoai.SelectedIndex = -1;
            txtGB.Text = "0";
            txtGN.Text = "0";
            txtSL.Text = "0";
            cmbS.SelectedIndex = -1;
            rdbC.Checked = true;
            picAvatar.Image = null;
            currentImageBytes = null;
            tenFileAnh = null;
        }

        private void ClearFormLoaiSP()
        {
            txtML.Text = "";
            txtTL.Text = "";
        }

        private void SetEditModeSanPham(bool enable)
        {
            txtID.ReadOnly = !enable; 
            txtSP.ReadOnly = !enable;
            cmbLoai.Enabled = enable;
            txtGB.ReadOnly = !enable;
            txtGN.ReadOnly = !enable;
            txtSL.ReadOnly = !enable;
            cmbS.Enabled = enable;
            rdbC.Enabled = enable;
            rdbH.Enabled = enable;
            btnCA.Enabled = enable;
            btnL.Enabled = enable;
            btnT.Enabled = !enable;
            btnS.Enabled = !enable;
            btnX.Enabled = !enable;
        }

        private void SetEditModeLoaiSP(bool enable)
        {
            txtML.ReadOnly = !enable; 
            txtTL.ReadOnly = !enable;
            btnL1.Enabled = enable;
            btnT1.Enabled = !enable;
            btnS1.Enabled = !enable;
            btnX1.Enabled = !enable;
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            if (imageIn == null) return null;
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
            using (var ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            try
            {
                cmbS.Items.Clear();
                cmbS.Items.Add("N");
                cmbS.Items.Add("V");
                cmbS.Items.Add("L");
                LoadCmbLoai();
                LoadLvSanPham();
                LoadLvLoaiSanPham();
                SetEditModeSanPham(false);
                SetEditModeLoaiSP(false);
                ClearFormSanPham();
                ClearFormLoaiSP();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvQLSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvQLSP.SelectedItems.Count > 0)
            {
                SANPHAM sp = (SANPHAM)lvQLSP.SelectedItems[0].Tag;
                
                if (sp != null)
                {
                    txtID.Text = sp.IDSP;
                    txtSP.Text = sp.TENSP;
                    cmbLoai.SelectedValue = sp.IDLOAI;
                    txtGB.Text = sp.GIABAN?.ToString();
                    txtGN.Text = sp.GIANHAP?.ToString();
                    txtSL.Text = sp.SOLUONG?.ToString();
                    cmbS.SelectedItem = sp.SIZE; 
                    if (sp.SOLUONG.GetValueOrDefault(0) > 0)
                    {
                        rdbC.Checked = true;
                    }
                    else
                    {
                        rdbH.Checked = true;
                    }
                    
                    if (sp.ANHSP != null && sp.ANHSP.Length > 0)
                    {
                        picAvatar.Image = ByteArrayToImage(sp.ANHSP);
                        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        picAvatar.Image = null; 
                    }

                    SetEditModeSanPham(false);
                }
            }
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            ClearFormSanPham();
            SetEditModeSanPham(true);
            txtID.ReadOnly = false; 
            txtID.Focus();
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            if (lvQLSP.SelectedItems.Count > 0)
            {
                SetEditModeSanPham(true);
                txtID.ReadOnly = true; 
                txtSP.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (lvQLSP.SelectedItems.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        string idSP = txtID.Text;
                        SANPHAM sp = db.SANPHAMs.Find(idSP); 

                        if (sp != null)
                        {
                            db.SANPHAMs.Remove(sp); 
                            db.SaveChanges(); 
                            LoadLvSanPham(); 
                            ClearFormSanPham();
                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnL_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtID.Text) ||
                    string.IsNullOrWhiteSpace(txtSP.Text) ||
                    cmbLoai.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ ID, Tên SP và chọn Loại SP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SANPHAM sp = db.SANPHAMs.Find(txtID.Text);
                bool isNew = (sp == null);

                if (isNew)
                {
                    sp = new SANPHAM();
                    sp.IDSP = txtID.Text;
                    db.SANPHAMs.Add(sp); 
                }
                sp.TENSP = txtSP.Text;
                sp.IDLOAI = cmbLoai.SelectedValue.ToString();

                if (double.TryParse(txtGB.Text, out double giaBan))
                    sp.GIABAN = giaBan;

                if (double.TryParse(txtGN.Text, out double giaNhap))
                    sp.GIANHAP = giaNhap;

                if (int.TryParse(txtSL.Text, out int soLuong))
                    sp.SOLUONG = soLuong;

                sp.SIZE = cmbS.SelectedItem?.ToString();

                if (rdbH.Checked)
                {
                    sp.SOLUONG = 0; 
                    sp.TRANGTHAI = false;
                }
                else if (sp.SOLUONG.GetValueOrDefault(0) > 0)
                {
                    sp.TRANGTHAI = true; 
                }
                else 
                {
                    sp.SOLUONG = 0;
                    sp.TRANGTHAI = false;
                    rdbH.Checked = true; 
                }

                txtSL.Text = sp.SOLUONG.ToString(); 
                if (picAvatar.Image != null)
                {
                    sp.ANHSP = ImageToByteArray(picAvatar.Image);
                }
                else
                {
                    sp.ANHSP = null; 
                }
                db.SaveChanges();
                sp.ANHSP = currentImageBytes;

                LoadLvSanPham();
                SetEditModeSanPham(false);
                MessageBox.Show(isNew ? "Thêm sản phẩm thành công!" : "Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (lvTTSP.SelectedItems.Count > 0)
            {
                LOAISP loai = (LOAISP)lvTTSP.SelectedItems[0].Tag;

                if (loai != null)
                {
                    txtML.Text = loai.IDLOAI;
                    txtTL.Text = loai.TENLOAI;
                    SetEditModeLoaiSP(false);
                }
            }
        }

        private void btnT1_Click(object sender, EventArgs e)
        {
            ClearFormLoaiSP();
            SetEditModeLoaiSP(true);
            txtML.ReadOnly = false;
            txtML.Focus();
        }

        private void btnS1_Click(object sender, EventArgs e)
        {
            if (lvTTSP.SelectedItems.Count > 0)
            {
                SetEditModeLoaiSP(true);
                txtML.ReadOnly = true;
                txtTL.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnX1_Click(object sender, EventArgs e)
        {
            if (lvTTSP.SelectedItems.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa loại này?\n(Xóa loại sẽ xóa TẤT CẢ sản phẩm thuộc loại đó!)", "Cảnh báo xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        string idLoai = txtML.Text;
                        LOAISP loai = db.LOAISPs.Find(idLoai);

                        if (loai != null)
                        {
                            db.LOAISPs.Remove(loai); 
                                                     
                            db.SaveChanges(); 

                            LoadLvLoaiSanPham();
                            LoadLvSanPham();
                            LoadCmbLoai();
                            ClearFormLoaiSP();

                            MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa loại sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnL1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtML.Text) || string.IsNullOrWhiteSpace(txtTL.Text))
                {
                    MessageBox.Show("Mã loại và Tên loại không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LOAISP loai = db.LOAISPs.Find(txtML.Text);
                bool isNew = (loai == null);

                if (isNew)
                {
                    loai = new LOAISP();
                    loai.IDLOAI = txtML.Text;
                    db.LOAISPs.Add(loai);
                }
                loai.TENLOAI = txtTL.Text;
                db.SaveChanges();
                LoadLvLoaiSanPham();
                LoadLvSanPham();
                LoadCmbLoai();

                SetEditModeLoaiSP(false);
                MessageBox.Show(isNew ? "Thêm loại thành công!" : "Cập nhật loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu loại sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmQuanLySanPham_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.Dispose();
        }

     
        private void btnTID_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTID.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadLvSanPham();
                    MessageBox.Show("Vui lòng nhập mã sản phẩm để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var listSanPham = db.SANPHAMs
                    .Include(sp => sp.LOAISP)
                    .Where(sp => sp.IDSP.Contains(keyword))
                    .ToList();

                lvQLSP.Items.Clear();

                if (listSanPham.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào có mã: " + keyword, "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var sp in listSanPham)
                {
                    ListViewItem lvi = new ListViewItem(sp.IDSP);
                    lvi.SubItems.Add(sp.TENSP);
                    lvi.SubItems.Add(sp.LOAISP?.TENLOAI ?? "");
                    lvi.SubItems.Add(sp.GIABAN?.ToString("N0"));
                    lvi.SubItems.Add(sp.GIANHAP?.ToString("N0"));
                    lvi.SubItems.Add(sp.SOLUONG?.ToString());
                    lvi.SubItems.Add(sp.SIZE);

                    string trangThai = (sp.SOLUONG.GetValueOrDefault(0) > 0) ? "Còn hàng" : "Hết hàng";
                    lvi.SubItems.Add(trangThai);

                    string moTaAnh = (sp.ANHSP != null && sp.ANHSP.Length > 0)
                        ? "Đã có ảnh minh họa"
                        : "(Chưa có ảnh minh họa)";
                    lvi.SubItems.Add(moTaAnh);

                    lvi.Tag = sp;

                    lvQLSP.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTID_Click(sender, e);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu frm = new frmMenu();
            frm.Show();
        }
    }
}
