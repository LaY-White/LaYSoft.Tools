using LaYSoft.BaseCode;
using LaYSoft.Tools.BLL.System;
using LaYSoft.Tools.Model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaYSoft.Tools
{
    public partial class wf_Test : Form
    {
        public wf_Test()
        {
            InitializeComponent();
        }

        private void Btn_ADD_Click(object sender, EventArgs e)
        {
            UserInfo User = new UserInfo();
            User.UserCode = "001";
            User.UserName = "Test";
            User.Password = "123";

            MyFactory<UserInfoBLL>.CreateCurSystemClass().Add(User);
            MessageBox.Show(User.UserID.ToString());
        }

        private void Btn_GetObject_Click(object sender, EventArgs e)
        {
            UserInfo User = new UserInfo();
            User.UserID = 2;
            User = MyFactory<UserInfoBLL>.CreateCurSystemClass().GetObject(User);
            MessageBox.Show(User.UserName);
        }

        private void Btn_Edit_Click(object sender, EventArgs e)
        {
            UserInfo User = new UserInfo();
            User.UserID = 2;
            User = MyFactory<UserInfoBLL>.CreateCurSystemClass().GetObject(User);

            User.UserName = "Test002";
            MyFactory<UserInfoBLL>.CreateCurSystemClass().Update(User);

            MessageBox.Show("OK");
        }

        private void Btn_Del_Click(object sender, EventArgs e)
        {
            UserInfo User = new UserInfo();
            User.UserID = 5;

            MyFactory<UserInfoBLL>.CreateCurSystemClass().Delete(User);

            MessageBox.Show("OK");
        }

        private void Btn_GetList_Click(object sender, EventArgs e)
        {
            UserInfoParams Pa = new UserInfoParams();
            Pa.Top = 5;
            Pa.BeginRow = 2;
            Pa.UserName = "Test";

            int Count = 0;
            IList<UserInfo> List = MyFactory<UserInfoBLL>.CreateCurSystemClass().GetListByPaged(Pa, out Count);

            MessageBox.Show(Count.ToString());
        }
    }
}
