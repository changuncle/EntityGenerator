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
            ChangeUsingColor();
        }

        /// <summary>
        /// 加载账户有权访问的数据库
        /// </summary>
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

        /// <summary>
        /// 加载指定数据库中的表
        /// </summary>
        /// <param name="database"></param>
        private void LoadTables(string database)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                StringBuilder tablesStringBuilder = new StringBuilder().AppendFormat("use {0};select * from sysobjects where xtype='U' order by name asc", GetFormatDatabaseName(database));
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
                //MessageBox.Show("数据表加载错误！错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDatabase_TextChanged(object sender, EventArgs e)
        {
            string current = cmbDatabase.Text;
            connString = Regex.Replace(connString, "Catalog=[0-9a-zA-Z_.]+;", "Catalog=" + current + ";");
            LoadTables(current);
        }

        /// <summary>
        /// 选择数据表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbTables_Click(object sender, EventArgs e)
        {
            GenerateEntity(lbTables.Text);
        }

        /// <summary>
        /// 修改命名空间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
            GenerateEntity(lbTables.Text);
        }

        /// <summary>
        /// 修改using的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsing_Leave(object sender, EventArgs e)
        {
            #region 统一using文本框中内容的字体
            string text = txtUsing.Text;
            txtUsing.Text = "";
            txtUsing.Text = text;
            #endregion
            ChangeKeyColor("using", Color.Blue, txtUsing);
        }

        /// <summary>
        /// 生成单类文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lbTables.Text.Trim()))
                {
                    MessageBox.Show("请选择要生成的表！");
                    return;
                }
                GenerateOneFile(lbTables.Text);
                MessageBox.Show("文件生成成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件生成失败！错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 生成所有类文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateAllFile_Click(object sender, EventArgs e)
        {
            foreach (DataRowView item in lbTables.Items)
            {
                string tableName = item.Row.ItemArray[0].ToString();
                //只生成特定前缀的表
                if (!string.IsNullOrEmpty(txtPrefixInfo.Text.Trim()) && tableName.StartsWith(txtPrefixInfo.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    GenerateEntity(tableName);
                    GenerateOneFile(tableName);
                }
                //生成所有表
                else if (string.IsNullOrEmpty(txtPrefixInfo.Text.Trim()))
                {
                    GenerateEntity(tableName);
                    GenerateOneFile(tableName);
                }
            }
            MessageBox.Show("文件生成成功！");
        }

        private void GenerateEntity(string tableName)
        {
            try
            {
                int length = 0;
                StringBuilder contentStringBuilder = new StringBuilder();
                string strSql = "select syscolumns.name as ColName,systypes.name as TypeName,sys.extended_properties.value as Description,sysobjects.name as TableName,syscolumns.isnullable from syscolumns " +
                                "inner join sysobjects on syscolumns.id=sysobjects.id " +
                                "inner join systypes on syscolumns.xtype=systypes.xtype " +
                                "left join sys.extended_properties on sys.extended_properties.major_id=syscolumns.id and sys.extended_properties.minor_id=syscolumns.colorder " +
                                "where sysobjects.name='" + tableName + "' and systypes.name<>'sysname' " +
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
                    string typeName = ds.Tables[0].Rows[i][1].ToString();
                    if (ds.Tables[0].Rows[i][4].ToString() == "1") typeName += "?";
                    contentStringBuilder.AppendLine("public " + typeName + " " + ds.Tables[0].Rows[i][0].ToString() + " { get; set; }");
                }
                contentStringBuilder.Append(new string(' ', length));
                contentStringBuilder.AppendLine("}");
                if (!string.IsNullOrEmpty(txtNamespace.Text.Trim()))
                {
                    contentStringBuilder.AppendLine("}");
                }
                string result = ChangeWords(contentStringBuilder.ToString());
                txtContent.Text = result;
                ChangeContentColor();
                adapter.Dispose();
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("类生成失败！错误信息：" + ex.Message);
            }
        }

        private void GenerateOneFile(string tableName)
        {
            if (string.IsNullOrEmpty(txtContent.Text.Trim()))
            {
                MessageBox.Show("生成内容不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(txtGeneratePath.Text.Trim()))
            {
                MessageBox.Show("生成路径不能为空！");
                txtGeneratePath.Focus();
                return;
            }
            string path = txtGeneratePath.Text.Trim();

            #region 按前缀生成文件时必须指定前缀分隔符
            if (cbxUsePrefix.Checked && string.IsNullOrEmpty(txtPrefixSplit.Text.Trim()))
            {
                MessageBox.Show("前缀分隔符不能为空！");
                txtPrefixSplit.Focus();
                return;
            }
            #endregion

            #region 按前缀生成文件时获取文件对应的存储路径
            if (cbxUsePrefix.Checked && !string.IsNullOrEmpty(txtPrefixSplit.Text.Trim()))
            {
                if (tableName.Trim().Contains(txtPrefixSplit.Text.Trim()))
                {
                    path = Path.Combine(path, tableName.Trim().Substring(0, tableName.Trim().IndexOf(txtPrefixSplit.Text.Trim())));
                }
            }
            #endregion

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, tableName.Trim() + ".cs");

            #region 不覆盖现有文件且文件已存在，则直接返回
            if (!cbxCover.Checked && File.Exists(filePath))
            {
                return;
            }
            #endregion

            StreamWriter sWriter = new StreamWriter(filePath, false, Encoding.Default);
            sWriter.Write(txtUsing.Text);
            sWriter.Write("\r\n\r\n");
            sWriter.Write(txtContent.Text);
            sWriter.Flush();
            sWriter.Close();
            sWriter.Dispose();
        }

        private string ChangeWords(string content)
        {
            string result = Regex.Replace(content, "nvarchar\\?", "string");
            result = Regex.Replace(result, "varchar\\?", "string");
            result = Regex.Replace(result, "nchar\\?", "string");
            result = Regex.Replace(result, "char\\?", "string");
            result = Regex.Replace(content, "nvarchar", "string");
            result = Regex.Replace(result, "varchar", "string");
            result = Regex.Replace(result, "nchar", "string");
            result = Regex.Replace(result, "char", "string");
            result = Regex.Replace(result, "uniqueidentifier", "Guid");
            result = Regex.Replace(result, "tinyint", "int");
            result = Regex.Replace(result, "smallint", "int");
            result = Regex.Replace(result, "bigint", "int");
            result = Regex.Replace(result, "datetime", "DateTime");
            result = Regex.Replace(result, "text", "string");
            result = Regex.Replace(result, "string\\?", "string");
            return result;
        }

        private void ChangeContentColor()
        {
            txtContent.SelectionStart = 0;
            txtContent.SelectionLength = txtContent.Text.Length;
            txtContent.SelectionColor = Color.Black;
            if (listDescription.Count > 0)
            {
                ChangeKeyColor(listDescription, Color.Green, txtContent);
            }
            ChangeKeyColor("namespace ", Color.Blue, txtContent);
            ChangeKeyColor("public ", Color.Blue, txtContent);
            ChangeKeyColor("class ", Color.Blue, txtContent);
            ChangeKeyColor("/// <summary>", Color.Gray, txtContent);
            ChangeKeyColor("/// ", Color.Gray, txtContent);
            ChangeKeyColor("/// </summary>", Color.Gray, txtContent);
            ChangeKeyColor("int ", Color.Blue, txtContent);
            ChangeKeyColor("int?", Color.Blue, txtContent);
            ChangeKeyColor("double ", Color.Blue, txtContent);
            ChangeKeyColor("double?", Color.Blue, txtContent);
            ChangeKeyColor("float ", Color.Blue, txtContent);
            ChangeKeyColor("float?", Color.Blue, txtContent);
            ChangeKeyColor("char ", Color.Blue, txtContent);
            ChangeKeyColor("string ", Color.Blue, txtContent);
            ChangeKeyColor("bool ", Color.Blue, txtContent);
            ChangeKeyColor("decimal ", Color.Blue, txtContent);
            ChangeKeyColor("enum ", Color.Blue, txtContent);
            ChangeKeyColor("const ", Color.Blue, txtContent);
            ChangeKeyColor("struct ", Color.Blue, txtContent);
            ChangeKeyColor("DateTime ", Color.CadetBlue, txtContent);
            ChangeKeyColor("DateTime?", Color.CadetBlue, txtContent);
            ChangeKeyColor("Guid ", Color.Green, txtContent);
            ChangeKeyColor("Guid?", Color.Green, txtContent);
            ChangeKeyColor("get", Color.Blue, txtContent);
            ChangeKeyColor("set", Color.Blue, txtContent);
        }

        private void ChangeUsingColor()
        {
            txtUsing.SelectionStart = 0;
            txtUsing.SelectionLength = txtContent.Text.Length;
            txtUsing.SelectionColor = Color.Black;
            ChangeKeyColor("using", Color.Blue, txtUsing);
        }

        public void ChangeKeyColor(string key, Color color, RichTextBox richTextBox)
        {
            key = FormatKey(key);
            Regex regex = new Regex(key);
            MatchCollection collection = regex.Matches(richTextBox.Text);
            foreach (Match match in collection)
            {
                richTextBox.SelectionStart = match.Index;
                richTextBox.SelectionLength = key.Length;
                richTextBox.SelectionColor = color;
            }
        }

        public void ChangeKeyColor(List<string> list, Color color, RichTextBox richTextBox)
        {
            foreach (string str in list)
            {
                ChangeKeyColor(str, color, richTextBox);
            }
        }

        private string FormatKey(string key)
        {
            if (key.Contains(@"\"))
            {
                key = key.Replace(@"\", @"\\");
            }
            if (key.Contains("?"))
            {
                key = key.Replace(@"?", @"\?");
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

        private string GetFormatDatabaseName(string database)
        {
            //针对SqlServer数据库
            return "[" + database + "]";
        }
    }
}
