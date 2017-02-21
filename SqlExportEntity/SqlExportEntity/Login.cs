using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SqlExportModels
{
    public partial class Login : Form
    {
        string connString = string.Empty;
        private Main main;
        public Login()
        {
            InitializeComponent();
        }
        public Login(Main main)
        {
            this.main = main;
            InitializeComponent();
            txtServer.Text = @".\sqlexpress";
            txtDatabase.Text = "Test";
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPassword.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (ckbSystemUser.Checked)
            {
                connString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=true;", txtServer.Text, txtDatabase.Text);
            }
            else
            {
                connString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=false;User={2};Password={3};", txtServer.Text, txtDatabase.Text, txtUser.Text, txtPassword.Text);
            }
            try
            {
                SqlConnection con = new SqlConnection(connString);
                con.Open();
                con.Close();
                main.connString = connString;
                this.Close();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                MessageBox.Show("连接失败！错误信息：" + ex.Message);
            }
        }

        private void ckbSystemUser_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbSystemUser.Checked)
            {
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
            }
        }
        /// <summary>
        /// 点击关闭按钮触发的事件
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            const int WM_SYSCOMMAND1 = 0x0112;
            const int SC_CLOSE1 = 0xF060;

            if (msg.Msg == WM_SYSCOMMAND1 && ((int)msg.WParam == SC_CLOSE1))
            {
                Application.Exit();
            }
            base.WndProc(ref msg);
        }
    }
}
