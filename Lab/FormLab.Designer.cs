namespace Lab
{
    partial class FormLab
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonTest = new System.Windows.Forms.Button();
            this.Log = new System.Windows.Forms.RichTextBox();
            this.ButtonUpdateData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonTest
            // 
            this.ButtonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonTest.Location = new System.Drawing.Point(12, 226);
            this.ButtonTest.Name = "ButtonTest";
            this.ButtonTest.Size = new System.Drawing.Size(75, 23);
            this.ButtonTest.TabIndex = 0;
            this.ButtonTest.Text = "Test";
            this.ButtonTest.UseVisualStyleBackColor = true;
            this.ButtonTest.Click += new System.EventHandler(this.ButtonTest_Click);
            // 
            // Log
            // 
            this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Log.Location = new System.Drawing.Point(13, 13);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(259, 207);
            this.Log.TabIndex = 1;
            this.Log.Text = "";
            // 
            // ButtonUpdateData
            // 
            this.ButtonUpdateData.Location = new System.Drawing.Point(102, 226);
            this.ButtonUpdateData.Name = "ButtonUpdateData";
            this.ButtonUpdateData.Size = new System.Drawing.Size(105, 23);
            this.ButtonUpdateData.TabIndex = 2;
            this.ButtonUpdateData.Text = "Update Data";
            this.ButtonUpdateData.UseVisualStyleBackColor = true;
            this.ButtonUpdateData.Click += new System.EventHandler(this.ButtonUpdateData_Click);
            // 
            // FormLab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ButtonUpdateData);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.ButtonTest);
            this.Name = "FormLab";
            this.Text = "Lab";
            this.Load += new System.EventHandler(this.FormLab_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonTest;
        private System.Windows.Forms.RichTextBox Log;
        private System.Windows.Forms.Button ButtonUpdateData;
    }
}

