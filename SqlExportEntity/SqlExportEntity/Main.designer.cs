namespace SqlExportModels
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.btnGenerateFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGeneratePath = new System.Windows.Forms.TextBox();
            this.txtUsing = new System.Windows.Forms.RichTextBox();
            this.btnGenerateAllFile = new System.Windows.Forms.Button();
            this.txtPrefixSplit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrefixInfo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxCover = new System.Windows.Forms.CheckBox();
            this.cbxUsePrefix = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "请选择数据库：";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(13, 31);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(200, 20);
            this.cmbDatabase.TabIndex = 2;
            this.cmbDatabase.TextChanged += new System.EventHandler(this.cmbDatabase_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "请选择数据表：";
            // 
            // lbTables
            // 
            this.lbTables.FormattingEnabled = true;
            this.lbTables.ItemHeight = 12;
            this.lbTables.Location = new System.Drawing.Point(14, 75);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(200, 160);
            this.lbTables.TabIndex = 4;
            this.lbTables.Click += new System.EventHandler(this.lbTables_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "命名空间：";
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(14, 260);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(200, 21);
            this.txtNamespace.TabIndex = 9;
            this.txtNamespace.Text = "DefaultNamespace";
            this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(220, 92);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(602, 436);
            this.txtContent.TabIndex = 10;
            this.txtContent.Text = "";
            // 
            // btnGenerateFile
            // 
            this.btnGenerateFile.Location = new System.Drawing.Point(102, 480);
            this.btnGenerateFile.Name = "btnGenerateFile";
            this.btnGenerateFile.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateFile.TabIndex = 11;
            this.btnGenerateFile.Text = "生成单类";
            this.btnGenerateFile.UseVisualStyleBackColor = true;
            this.btnGenerateFile.Click += new System.EventHandler(this.btnGenerateFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "生成路径：";
            // 
            // txtGeneratePath
            // 
            this.txtGeneratePath.Location = new System.Drawing.Point(13, 304);
            this.txtGeneratePath.Name = "txtGeneratePath";
            this.txtGeneratePath.Size = new System.Drawing.Size(200, 21);
            this.txtGeneratePath.TabIndex = 13;
            this.txtGeneratePath.Text = "D:\\EntityModels";
            // 
            // txtUsing
            // 
            this.txtUsing.Location = new System.Drawing.Point(220, 5);
            this.txtUsing.Name = "txtUsing";
            this.txtUsing.Size = new System.Drawing.Size(602, 80);
            this.txtUsing.TabIndex = 14;
            this.txtUsing.Text = "using System;\nusing System.Text;\nusing System.Collections.Generic;\nusing System.C" +
    "omponentModel.DataAnnotations;";
            this.txtUsing.Leave += new System.EventHandler(this.txtUsing_Leave);
            // 
            // btnGenerateAllFile
            // 
            this.btnGenerateAllFile.Location = new System.Drawing.Point(14, 480);
            this.btnGenerateAllFile.Name = "btnGenerateAllFile";
            this.btnGenerateAllFile.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateAllFile.TabIndex = 15;
            this.btnGenerateAllFile.Text = "生成所有类";
            this.btnGenerateAllFile.UseVisualStyleBackColor = true;
            this.btnGenerateAllFile.Click += new System.EventHandler(this.btnGenerateAllFile_Click);
            // 
            // txtPrefixSplit
            // 
            this.txtPrefixSplit.Location = new System.Drawing.Point(12, 348);
            this.txtPrefixSplit.Name = "txtPrefixSplit";
            this.txtPrefixSplit.Size = new System.Drawing.Size(200, 21);
            this.txtPrefixSplit.TabIndex = 17;
            this.txtPrefixSplit.Text = "_";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 333);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "前缀分隔符：";
            // 
            // txtPrefixInfo
            // 
            this.txtPrefixInfo.Location = new System.Drawing.Point(12, 392);
            this.txtPrefixInfo.Name = "txtPrefixInfo";
            this.txtPrefixInfo.Size = new System.Drawing.Size(200, 21);
            this.txtPrefixInfo.TabIndex = 19;
            this.txtPrefixInfo.Text = "Sys_";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 377);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "只生成特定前缀（生成所有类）：";
            // 
            // cbxCover
            // 
            this.cbxCover.AutoSize = true;
            this.cbxCover.Checked = true;
            this.cbxCover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxCover.Location = new System.Drawing.Point(11, 422);
            this.cbxCover.Name = "cbxCover";
            this.cbxCover.Size = new System.Drawing.Size(96, 16);
            this.cbxCover.TabIndex = 20;
            this.cbxCover.Text = "覆盖现有文件";
            this.cbxCover.UseVisualStyleBackColor = true;
            // 
            // cbxUsePrefix
            // 
            this.cbxUsePrefix.AutoSize = true;
            this.cbxUsePrefix.Location = new System.Drawing.Point(11, 445);
            this.cbxUsePrefix.Name = "cbxUsePrefix";
            this.cbxUsePrefix.Size = new System.Drawing.Size(144, 16);
            this.cbxUsePrefix.TabIndex = 21;
            this.cbxUsePrefix.Text = "按前缀生成到对应目录";
            this.cbxUsePrefix.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 540);
            this.Controls.Add(this.cbxUsePrefix);
            this.Controls.Add(this.cbxCover);
            this.Controls.Add(this.txtPrefixInfo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPrefixSplit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGenerateAllFile);
            this.Controls.Add(this.txtUsing);
            this.Controls.Add(this.txtGeneratePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGenerateFile);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbTables);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "实体类生成工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Button btnGenerateFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGeneratePath;
        private System.Windows.Forms.RichTextBox txtUsing;
        private System.Windows.Forms.Button btnGenerateAllFile;
        private System.Windows.Forms.TextBox txtPrefixSplit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrefixInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbxCover;
        private System.Windows.Forms.CheckBox cbxUsePrefix;
    }
}

