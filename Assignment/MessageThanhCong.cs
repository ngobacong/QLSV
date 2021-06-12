using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class MessageThanhCong : Form
    {
        public MessageThanhCong()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            string loai = DangNhap.type;
            if (loai == "giangvien")
            {
                QL_Diem quanLy = new QL_Diem();
                quanLy.Show();
                this.Close();
            }
            else
            {
                QL_SinhVien sv = new QL_SinhVien();
                sv.Show();
                this.Close();
            }
        }
    }
}
