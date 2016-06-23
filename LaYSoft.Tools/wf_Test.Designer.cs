namespace LaYSoft.Tools
{
    partial class wf_Test
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
            this.Btn_ADD = new System.Windows.Forms.Button();
            this.Btn_Edit = new System.Windows.Forms.Button();
            this.Btn_Del = new System.Windows.Forms.Button();
            this.Btn_GetObject = new System.Windows.Forms.Button();
            this.Btn_GetList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_ADD
            // 
            this.Btn_ADD.Location = new System.Drawing.Point(35, 40);
            this.Btn_ADD.Name = "Btn_ADD";
            this.Btn_ADD.Size = new System.Drawing.Size(75, 23);
            this.Btn_ADD.TabIndex = 0;
            this.Btn_ADD.Text = "新增";
            this.Btn_ADD.UseVisualStyleBackColor = true;
            this.Btn_ADD.Click += new System.EventHandler(this.Btn_ADD_Click);
            // 
            // Btn_Edit
            // 
            this.Btn_Edit.Location = new System.Drawing.Point(35, 69);
            this.Btn_Edit.Name = "Btn_Edit";
            this.Btn_Edit.Size = new System.Drawing.Size(75, 23);
            this.Btn_Edit.TabIndex = 1;
            this.Btn_Edit.Text = "修改";
            this.Btn_Edit.UseVisualStyleBackColor = true;
            this.Btn_Edit.Click += new System.EventHandler(this.Btn_Edit_Click);
            // 
            // Btn_Del
            // 
            this.Btn_Del.Location = new System.Drawing.Point(35, 98);
            this.Btn_Del.Name = "Btn_Del";
            this.Btn_Del.Size = new System.Drawing.Size(75, 23);
            this.Btn_Del.TabIndex = 2;
            this.Btn_Del.Text = "删除";
            this.Btn_Del.UseVisualStyleBackColor = true;
            this.Btn_Del.Click += new System.EventHandler(this.Btn_Del_Click);
            // 
            // Btn_GetObject
            // 
            this.Btn_GetObject.Location = new System.Drawing.Point(142, 51);
            this.Btn_GetObject.Name = "Btn_GetObject";
            this.Btn_GetObject.Size = new System.Drawing.Size(75, 23);
            this.Btn_GetObject.TabIndex = 3;
            this.Btn_GetObject.Text = "获取实例";
            this.Btn_GetObject.UseVisualStyleBackColor = true;
            this.Btn_GetObject.Click += new System.EventHandler(this.Btn_GetObject_Click);
            // 
            // Btn_GetList
            // 
            this.Btn_GetList.Location = new System.Drawing.Point(142, 80);
            this.Btn_GetList.Name = "Btn_GetList";
            this.Btn_GetList.Size = new System.Drawing.Size(75, 23);
            this.Btn_GetList.TabIndex = 4;
            this.Btn_GetList.Text = "获取列表";
            this.Btn_GetList.UseVisualStyleBackColor = true;
            this.Btn_GetList.Click += new System.EventHandler(this.Btn_GetList_Click);
            // 
            // wf_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 223);
            this.Controls.Add(this.Btn_GetList);
            this.Controls.Add(this.Btn_GetObject);
            this.Controls.Add(this.Btn_Del);
            this.Controls.Add(this.Btn_Edit);
            this.Controls.Add(this.Btn_ADD);
            this.Name = "wf_Test";
            this.ShowIcon = false;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ADD;
        private System.Windows.Forms.Button Btn_Edit;
        private System.Windows.Forms.Button Btn_Del;
        private System.Windows.Forms.Button Btn_GetObject;
        private System.Windows.Forms.Button Btn_GetList;
    }
}

