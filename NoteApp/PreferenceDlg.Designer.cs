namespace NoteApp
{
    partial class PreferenceDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLocalCache = new System.Windows.Forms.TextBox();
            this.textBoxServerCache = new System.Windows.Forms.TextBox();
            this.buttonBrowseLocal = new System.Windows.Forms.Button();
            this.buttonServerBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Local Cache";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server DB";
            // 
            // textBoxLocalCache
            // 
            this.textBoxLocalCache.Location = new System.Drawing.Point(190, 70);
            this.textBoxLocalCache.Name = "textBoxLocalCache";
            this.textBoxLocalCache.Size = new System.Drawing.Size(286, 22);
            this.textBoxLocalCache.TabIndex = 2;
            // 
            // textBoxServerCache
            // 
            this.textBoxServerCache.Location = new System.Drawing.Point(190, 109);
            this.textBoxServerCache.Name = "textBoxServerCache";
            this.textBoxServerCache.Size = new System.Drawing.Size(286, 22);
            this.textBoxServerCache.TabIndex = 3;
            // 
            // buttonBrowseLocal
            // 
            this.buttonBrowseLocal.Location = new System.Drawing.Point(482, 70);
            this.buttonBrowseLocal.Name = "buttonBrowseLocal";
            this.buttonBrowseLocal.Size = new System.Drawing.Size(40, 23);
            this.buttonBrowseLocal.TabIndex = 4;
            this.buttonBrowseLocal.Text = "...";
            this.buttonBrowseLocal.UseVisualStyleBackColor = true;
            this.buttonBrowseLocal.Click += new System.EventHandler(this.buttonBrowseLocal_Click);
            // 
            // buttonServerBrowse
            // 
            this.buttonServerBrowse.Location = new System.Drawing.Point(482, 108);
            this.buttonServerBrowse.Name = "buttonServerBrowse";
            this.buttonServerBrowse.Size = new System.Drawing.Size(40, 23);
            this.buttonServerBrowse.TabIndex = 5;
            this.buttonServerBrowse.Text = "...";
            this.buttonServerBrowse.UseVisualStyleBackColor = true;
            this.buttonServerBrowse.Click += new System.EventHandler(this.buttonServerBrowse_Click);
            // 
            // PreferenceDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 317);
            this.Controls.Add(this.buttonServerBrowse);
            this.Controls.Add(this.buttonBrowseLocal);
            this.Controls.Add(this.textBoxServerCache);
            this.Controls.Add(this.textBoxLocalCache);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PreferenceDlg";
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLocalCache;
        private System.Windows.Forms.TextBox textBoxServerCache;
        private System.Windows.Forms.Button buttonBrowseLocal;
        private System.Windows.Forms.Button buttonServerBrowse;
    }
}