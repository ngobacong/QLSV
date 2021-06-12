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
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        SqlConnection Connection = new SqlConnection(@"Data Source=CÔNG;Initial Catalog=FPL_DaoTao;Integrated Security=True");
        public static string type;

        public void check()
        {
            string user = name.Text;
            string pass = mk.Text;
            string sqlselect = "Select * from USERS where Username = '" + user + "' and Password = '" + pass + "'";
            Connection.Open();
            SqlCommand cmd = new SqlCommand(sqlselect, Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                Hide();
                MessageThanhCong success = new MessageThanhCong();
                success.Show();
                type = reader.GetString(reader.GetOrdinal("Role"));
            }
            else
            {
                MessageThatBai failed = new MessageThatBai();
                failed.Show();
                name.Text = "";
                mk.Text = "";
            }
            Connection.Close();
        }

        private void login_Click(object sender, EventArgs e)
        {
            check();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
