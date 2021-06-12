using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class QL_SinhVien : Form
    {
        public QL_SinhVien()
        {
            InitializeComponent();
            ConnectDatabse();
        }
        SqlConnection con = new SqlConnection(@"Data Source=công;Initial Catalog=FPL_DaoTao;Integrated Security=True");
        private void ConnectDatabse()
        {
            string query = "Select * from STUDENTS";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            kq.DataSource = dt;
            con.Close();
        }

        private void bt_close(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
      

        private void QL_SinhVien_Load(object sender, EventArgs e)
        {
            mssv.ReadOnly = true;
            ten.ReadOnly = true;
            email.ReadOnly = true;
            sdt.ReadOnly = true;
            diachi.ReadOnly = true;
            ConnectDatabse();
        }

        private void add_Click(object sender, EventArgs e)
        {
            mssv.ReadOnly = false;
            ten.ReadOnly = false;
            email.ReadOnly = false;
            sdt.ReadOnly = false;
            diachi.ReadOnly = false;
            mssv.ResetText();
            ten.ResetText();
            email.ResetText();
            sdt.ResetText();
            rdbtNam.Checked = false;
            rdbtNu.Checked = false;
            diachi.ResetText();
            picture.Image = null;
        }

        private void save_Click(object sender, EventArgs e)
        {
            string sqlInsert = "insert into STUDENTS values(@MaSV,@Hoten,@Email,@SoDT,@Gioitinh,@Diachi,@hinh)";
            con.Open();
            SqlCommand com = new SqlCommand(sqlInsert, con);

            com.Parameters.AddWithValue("@MaSV", mssv.Text);
            com.Parameters.AddWithValue("@Hoten", ten.Text);
            com.Parameters.AddWithValue("@Email", email.Text);
            com.Parameters.AddWithValue("@SoDT", sdt.Text);
            if (rdbtNam.Checked == true)
            {
                com.Parameters.AddWithValue("@Gioitinh", 1);
            }
            else if (rdbtNu.Checked == true)
            {
                com.Parameters.AddWithValue("@Gioitinh", 0);
            }
            com.Parameters.AddWithValue("@Diachi", diachi.Text);
            com.Parameters.AddWithValue("@hinh", null);
            try
            {
                com.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!");
            }
            catch
            {
                MessageBox.Show("Thất bại!");
            }
            finally
            {
                con.Close();
            }
            ConnectDatabse();
        }


        private void up_Click(object sender, EventArgs e)
        {
            mssv.ReadOnly = false;
            ten.ReadOnly = false;
            email.ReadOnly = false;
            sdt.ReadOnly = false;
            diachi.ReadOnly = false;
        }

        private void picture_Click(object sender, EventArgs e)
        {
            OpenFileDialog uploadFileSteam = new OpenFileDialog();

            uploadFileSteam.InitialDirectory = "D:\\C#_3\\";

            if (uploadFileSteam.ShowDialog() == DialogResult.OK)
            {
                File.Copy(uploadFileSteam.FileName, Directory.GetCurrentDirectory() + "\\Resources\\" + uploadFileSteam.SafeFileName);
            }
        }

        private void kq_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.kq.Rows[e.RowIndex];

                mssv.Text = row.Cells[0].Value.ToString();
                ten.Text = row.Cells[1].Value.ToString();
                email.Text = row.Cells[2].Value.ToString();
                sdt.Text = row.Cells[3].Value.ToString();
                if (row.Cells[4].Value.GetHashCode() == 1)
                {
                    rdbtNam.Checked = true;
                    rdbtNu.Checked = false;
                }
                else
                {
                    rdbtNu.Checked = true;
                    rdbtNam.Checked = false;
                }
                diachi.Text = row.Cells[5].Value.ToString();
                if (row.Cells[6].Value != null)
                {
                    picture.Image = new Bitmap(Application.StartupPath + "\\Resources\\" + row.Cells[6].Value.ToString());
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xoá không?","Thông báo",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sqlDelete = "delete from STUDENTS where MaSV = @MaSV";
                con.Open();
                SqlCommand com = new SqlCommand(sqlDelete, con);

                com.Parameters.AddWithValue("@MaSV", mssv.Text);
                try
                {
                    com.ExecuteNonQuery();
                    MessageBox.Show("Xoá thành công!");
                }
                catch
                {
                    MessageBox.Show("Thất bại!");
                }
                finally
                {
                    con.Close();
                }
                ConnectDatabse();
            }    
        }
        public void updateValues()
        {
            string sqlUpdate = "update STUDENTS set Hoten = @Hoten, Email = @Email, SoDT = @SoDT, GioiTinh = @GioiTinh, Diachi = @Diachi where MaSV = @MaSV";
            con.Open();
            SqlCommand com = new SqlCommand(sqlUpdate, con);

            com.Parameters.AddWithValue("@MaSV", mssv.Text);
            com.Parameters.AddWithValue("@Hoten", ten.Text);
            com.Parameters.AddWithValue("@Email", email.Text);
            com.Parameters.AddWithValue("@SoDT", sdt.Text);
            if(rdbtNam.Checked == true && rdbtNu.Checked == false)
            {
                com.Parameters.AddWithValue("@Gioitinh", 1);
            }    
            else if (rdbtNu.Checked == true && rdbtNam.Checked == false)
            {
                com.Parameters.AddWithValue("@Gioitinh", 0);
            }
            com.Parameters.AddWithValue("@Diachi", diachi.Text);

            try
            {
                com.ExecuteNonQuery();
                MessageBox.Show("Update thành công!");
            }
            catch 
            {
                MessageBox.Show("Thất bại!");
            }
            finally
            {
                con.Close();
            }
        }
    }
}
