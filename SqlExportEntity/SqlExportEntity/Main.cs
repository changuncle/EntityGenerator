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
using System.IO;

namespace SqlExportModels
{
    public partial class Main : Form
    {
        public string connString = string.Empty;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        List<string> listDescription = new List<string>();
        public Main()
        {
            InitializeComponent();
            Login login = new Login(this);
            login.ShowDialog();
            LoadDatabase();

        }

        private void LoadDatabase()
        {
            try
            {
                con = new SqlConnection(connString);
                con.Open();
                string sqlDatabase = "use master;select * from sysdatabases order by name asc";
                cmd = new SqlCommand(sqlDatabase, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);
                cmbDatabase.DisplayMember = "name";
                cmbDatabase.ValueMember = "name";
                //数据源绑定在后可避免System.Data.DataRowView的问题
                cmbDatabase.DataSource = ds.Tables[0];
                adapter.Dispose();
                cmd.Dispose();
                con.Close();
                connString = Regex.Replace(connString, "Catalog=[0-9a-zA-Z_]+;", "Catalog=" + cmbDatabase.Text + ";");
                LoadTables(cmbDatabase.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库加载错误！错误信息：" + ex.Message);
            }
        }

        private void LoadTables(string database)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                { 
                    con.Open(); 
                }
                StringBuilder tablesStringBuilder = new StringBuilder().AppendFormat("use {0};select * from sysobjects where xtype='U' order by name asc", database);
                cmd = new SqlCommand(tablesStringBuilder.ToString(), con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);
                lbTables.DataSource = ds.Tables[0];
                lbTables.DisplayMember = "name";
                lbTables.ValueMember = "name";
                //【1】取消默认选中项
                this.lbTables.SelectedItems.Clear();
                //【2】取消默认选中项
                //lbTables.SetSelected(0, false);
                adapter.Dispose();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据表加载错误！错误信息：" + ex.Message);
            }
        }

        private void cmbDatabase_TextChanged(object sender, EventArgs e)
        {
            string current = cmbDatabase.Text;
            connString = Regex.Replace(connString, "Catalog=[0-9a-zA-Z_]+;", "Catalog=" + current + ";");
            LoadTables(current);
        }

        private string ChangeWords(string content)
        {
            string result = Regex.Replace(content, "nvarchar", "string");
            result = Regex.Replace(result, "varchar", "string");
            result = Regex.Replace(result, "nchar", "string");
            result = Regex.Replace(result, "char", "string");
            result = Regex.Replace(result, "tinyint", "int");
            result = Regex.Replace(result, "smallint", "int");
            result = Regex.Replace(result, "bigint", "int");
            result = Regex.Replace(result, "datetime", "DateTime");
            result = Regex.Replace(result, "text", "string");
            return result;
        }

        private void ChangeColor()
        {
            txtContent.SelectionStart = 0;
            txtContent.SelectionLength = txtContent.Text.Length;
            txtContent.SelectionColor = Color.Black;
            if (listDescription.Count > 0)
            { 
                ChangeKeyColor(listDescription, Color.Green);  
            }
            ChangeKeyColor("namespace ", Color.Blue);
            ChangeKeyColor("public ", Color.Blue);
            ChangeKeyColor("class ", Color.Blue);
            ChangeKeyColor("/// <summary>",Color.Gray);
            ChangeKeyColor("/// ", Color.Gray);
            ChangeKeyColor("/// </summary>", Color.Gray);
            ChangeKeyColor("int ", Color.Blue);
            ChangeKeyColor("double ", Color.Blue);
            ChangeKeyColor("float ", Color.Blue);
            ChangeKeyColor("char ", Color.Blue);
            ChangeKeyColor("string ", Color.Blue);
            ChangeKeyColor("bool ", Color.Blue);
            ChangeKeyColor("decimal ", Color.Blue);
            ChangeKeyColor("enum ", Color.Blue);
            ChangeKeyColor("const ", Color.Blue);
            ChangeKeyColor("struct ", Color.Blue);
            ChangeKeyColor("DateTime ", Color.CadetBlue);
            ChangeKeyColor("get",Color.Blue);
            ChangeKeyColor("set", Color.Blue);
        }

        public void ChangeKeyColor(string key, Color color)
        {
            key = FormatKey(key);
            Regex regex = new Regex(key);
            MatchCollection collection = regex.Matches(txtContent.Text);
            foreach (Match match in collection)
            {
                txtContent.SelectionStart = match.Index;
                txtContent.SelectionLength = key.Length;
                txtContent.SelectionColor = color;
            }
        }

        public void ChangeKeyColor(List<string> list, Color color)
        {
            foreach (string str in list)
            {
                ChangeKeyColor(str, color);
            }
        }

        private string FormatKey(string key)
        {
            if (key.Contains(@"\"))
            {
                key = key.Replace(@"\", @"\\");
            }
            if (key.Contains("("))
            {
                key = key.Replace("(", "\\(");
            }
            if (key.Contains(")"))
            {
                key = key.Replace(")", "\\)");
            }
            return key;
        }

        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            try 
            {
                if (string.IsNullOrEmpty(txtContent.Text.Trim()))
                {
                    MessageBox.Show("生成内容不能为空！"); 
                    return;
                }
                if (string.IsNullOrEmpty(lbTables.Text.Trim()))
                {
                    MessageBox.Show("请选择要生成的表！");
                    return;
                }
                if(string.IsNullOrEmpty(txtGeneratePath.Text.Trim()))
                {
                    MessageBox.Show("生成路径不能为空！");
                    return;
                }
                string path = txtGeneratePath.Text.Trim();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, lbTables.Text.Trim() + ".cs");
                StreamWriter sWriter = new StreamWriter(filePath, false, Encoding.Default);
                sWriter.Write(txtContent.Text);
                sWriter.Flush();
                sWriter.Close();
                sWriter.Dispose();
                MessageBox.Show("文件生成成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("文件生成失败！错误信息：" + ex.Message);
            }
        }

        private void GenerateEntity()
        {
            try
            {
                int length = 0;
                StringBuilder contentStringBuilder = new StringBuilder();
                string strSql = "select syscolumns.name as ColName,systypes.name as TypeName,sys.extended_properties.value as Description,sysobjects.name as TableName from syscolumns " +
                                "inner join sysobjects on syscolumns.id=sysobjects.id " +
                                "inner join systypes on syscolumns.xtype=systypes.xtype " +
                                "left join sys.extended_properties on sys.extended_properties.major_id=syscolumns.id and sys.extended_properties.minor_id=syscolumns.colorder " +
                                "where sysobjects.name='" + lbTables.Text + "' and systypes.name<>'sysname' " +
                                "order by sys.extended_properties.minor_id asc";
                con = new SqlConnection(connString);
                con.Open();
                cmd = new SqlCommand(strSql, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "Entity");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    txtContent.Text = "没有查询结果......";
                    return;
                }

                if (!string.IsNullOrEmpty(txtNamespace.Text.Trim()))
                {
                    contentStringBuilder.AppendLine("namespace " + txtNamespace.Text + "\r\n{");
                    length += 4;
                }
                contentStringBuilder.Append(new string(' ', length));
                contentStringBuilder.AppendLine("public class " + ds.Tables[0].Rows[0][3].ToString());
                contentStringBuilder.Append(new string(' ', length));
                contentStringBuilder.AppendLine("{");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i][2].ToString()))
                    {
                        contentStringBuilder.Append(new string(' ', length + 4));
                        contentStringBuilder.AppendLine("/// <summary>");
                        contentStringBuilder.Append(new string(' ', length + 4));
                        contentStringBuilder.AppendLine("/// " + ds.Tables[0].Rows[i][2]);
                        contentStringBuilder.Append(new string(' ', length + 4));
                        contentStringBuilder.AppendLine("/// </summary>");
                        if (!listDescription.Contains(ds.Tables[0].Rows[i][2].ToString()))
                        {
                            listDescription.Add(ds.Tables[0].Rows[i][2].ToString());
                        }
                    }
                    contentStringBuilder.Append(new string(' ', length + 4));
                    contentStringBuilder.AppendLine("public " + ds.Tables[0].Rows[i][1].ToString() + " " + ds.Tables[0].Rows[i][0].ToString() + " { get; set; }");
                }
                contentStringBuilder.Append(new string(' ', length));
                contentStringBuilder.AppendLine("}");
                if (!string.IsNullOrEmpty(txtNamespace.Text.Trim()))
                {
                    contentStringBuilder.AppendLine("}");
                }
                string result = ChangeWords(contentStringBuilder.ToString());
                txtContent.Text = result;
                ChangeColor();
                adapter.Dispose();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("类生成失败！错误信息：" + ex.Message);
            }
        }

        private void lbTables_Click(object sender, EventArgs e)
        {
            GenerateEntity();
        }

        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
            GenerateEntity();
        }
    }
}
