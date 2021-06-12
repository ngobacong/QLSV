using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class QL_Diem : Form
    {
        public QL_Diem()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=công;Initial Catalog=FPL_DaoTao;Integrated Security=True");
        DataTable datatable = new DataTable();
        BindingSource bindingsource;
        private void ConnectDatabse()
        {
            string query = "Select top(3) ID,STUDENTS.MaSV,Hoten,Tienganh,Tinhoc,GDTC,(Tienganh+Tinhoc+GDTC)/3 as 'DiemTB'" +
                "from STUDENTS inner join GRADE on STUDENTS.MaSV = GRADE.MaSV order by DiemTB desc";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            datatable = new DataTable();
            datatable.Load(cmd.ExecuteReader());
            bindingsource = new BindingSource();
            bindingsource.DataSource = datatable;
            da.Fill(dt);
            kq.DataSource = dt;
            con.Close();
        }
        public void UpdateDatabase()
        {
            string sqlUpdate = "Update GRADE set Tienganh = @Tienganh, Tinhoc = @Tinhoc, GDTC = @GDTC where MaSV = @MaSV";
            con.Open();
            SqlCommand com = new SqlCommand(sqlUpdate, con);

            com.Parameters.AddWithValue("@MaSV", mssv.Text);
            com.Parameters.AddWithValue("@Tienganh", ta.Text);
            com.Parameters.AddWithValue("@Tinhoc", tin.Text);
            com.Parameters.AddWithValue("@GDTC", GDTC.Text);
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

        private void bt_close(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dgvDiemCao_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.kq.Rows[e.RowIndex];

                tim.Text = row.Cells[0].Value.ToString();
                mssv.Text = row.Cells[1].Value.ToString();
                ten.Text = row.Cells[2].Value.ToString();
                ta.Text = row.Cells[3].Value.ToString();
                tin.Text = row.Cells[4].Value.ToString();
                GDTC.Text = row.Cells[5].Value.ToString();
                diemtb.Text = row.Cells[6].Value.ToString();
            }
        }

        private void ShowRecord()
        {
            tim.Text = kq.Rows[kq.CurrentRow.Index].Cells[0].Value.ToString();
            mssv.Text = kq.Rows[kq.CurrentRow.Index].Cells[1].Value.ToString();
            ten.Text = kq.Rows[kq.CurrentRow.Index].Cells[2].Value.ToString();
            ta.Text = kq.Rows[kq.CurrentRow.Index].Cells[3].Value.ToString();
            tin.Text = kq.Rows[kq.CurrentRow.Index].Cells[4].Value.ToString();
            GDTC.Text = kq.Rows[kq.CurrentRow.Index].Cells[5].Value.ToString();
            diemtb.Text = kq.Rows[kq.CurrentRow.Index].Cells[6].Value.ToString();
        }

        private void QL_Diem_Load(object sender, EventArgs e)
        {
            ten.ReadOnly = true;
            
            mssv.ReadOnly = true;
            ta.ReadOnly = true;
            tin.ReadOnly = true;
            GDTC.ReadOnly = true;
            add.Enabled = false;
            ConnectDatabse();
        }

        private void search_Click(object sender, EventArgs e)
        {
            string search = tim.Text;
            string sqlSearch = "Select ID,STUDENTS.MaSV,Hoten,Tienganh,Tinhoc,GDTC,(Tienganh+Tinhoc+GDTC)/3 as N'Điểm TB' " +
                "from STUDENTS inner join GRADE on STUDENTS.MaSV = GRADE.MaSV where STUDENTS.MaSV like '" + search + "%'";
            con.Open();
            SqlCommand cmd = new SqlCommand(sqlSearch, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            kq.DataSource = dt;
            if (search == "")
            {
                kq.DataSource = null;
                kq.Refresh();
            }
            con.Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            UpdateDatabase();
            ConnectDatabse();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete row?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                mssv.ReadOnly = false;
                string sqlDelete = "Update GRADE set Tienganh = @Tienganh, Tinhoc = @Tinhoc, GDTC = @GDTC where MaSV = @MaSV";

                con.Open();
                SqlCommand sqlcmd = new SqlCommand(sqlDelete, con);

                sqlcmd.Parameters.AddWithValue("@MaSV", mssv.Text);
                sqlcmd.Parameters.AddWithValue("@Tienganh", 0);
                sqlcmd.Parameters.AddWithValue("@Tinhoc", 0);
                sqlcmd.Parameters.AddWithValue("@GDTC", 0);

                sqlcmd.ExecuteNonQuery();
                con.Close();
                ConnectDatabse();
            }
        }

        private void up_Click(object sender, EventArgs e)
        {
            mssv.ReadOnly = false;
            ta.ReadOnly = false;
            tin.ReadOnly = false;
            GDTC.ReadOnly = false;
        }

        private void btFirst_Click(object sender, EventArgs e)
        {
            kq.ClearSelection();
            kq.Rows[0].Selected = true;
            bindingsource.DataSource = datatable;
            kq.DataSource = bindingsource;
            bindingsource.MoveFirst();

            ShowRecord();
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            try
            {
                int IndexNow = kq.CurrentRow.Index;

                if (kq.Rows.Count > IndexNow)
                {
                    kq.Rows[IndexNow + 1].Selected = true;
                }

                bindingsource.DataSource = datatable;
                kq.DataSource = bindingsource;
                bindingsource.MoveNext();
            }
            catch (Exception)
            {

            }

            ShowRecord();
        }

        private void btPrev_Click(object sender, EventArgs e)
        {
            try
            {
                int IndexNow = kq.CurrentRow.Index;

                if (kq.Rows.Count > IndexNow)
                {
                    kq.Rows[IndexNow - 1].Selected = true;
                }

                bindingsource.DataSource = datatable;
                kq.DataSource = bindingsource;
                bindingsource.MovePrevious();
            }
            catch (Exception)
            {

            }
            ShowRecord();
        }

        private void btLast_Click(object sender, EventArgs e)
        {
            kq.ClearSelection();
            int so = kq.Rows.Count - 1;
            kq.Rows[so - 1].Selected = true;
            bindingsource.DataSource = datatable;
            kq.DataSource = bindingsource;
            bindingsource.MoveLast();
            ShowRecord();
        }
    }
}
