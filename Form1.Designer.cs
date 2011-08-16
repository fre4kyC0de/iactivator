namespace iActivator
{
    partial class Form1
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
            this.activate_btn = new System.Windows.Forms.Button();
            this.deactivate_btn = new System.Windows.Forms.Button();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpCache = new System.Windows.Forms.GroupBox();
            this.activationticket = new System.Windows.Forms.CheckBox();
            this.activationdata = new System.Windows.Forms.CheckBox();
            this.cache_btn = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.SaveFileDialog();
            this.Open = new System.Windows.Forms.OpenFileDialog();
            this.StatusBar.SuspendLayout();
            this.grpCache.SuspendLayout();
            this.SuspendLayout();
            // 
            // activate_btn
            // 
            this.activate_btn.Location = new System.Drawing.Point(149, 12);
            this.activate_btn.Name = "activate_btn";
            this.activate_btn.Size = new System.Drawing.Size(131, 27);
            this.activate_btn.TabIndex = 3;
            this.activate_btn.TabStop = false;
            this.activate_btn.Text = "Activate";
            this.activate_btn.UseVisualStyleBackColor = true;
            this.activate_btn.Click += new System.EventHandler(this.activate_btn_Click);
            // 
            // deactivate_btn
            // 
            this.deactivate_btn.Location = new System.Drawing.Point(149, 45);
            this.deactivate_btn.Name = "deactivate_btn";
            this.deactivate_btn.Size = new System.Drawing.Size(131, 27);
            this.deactivate_btn.TabIndex = 4;
            this.deactivate_btn.TabStop = false;
            this.deactivate_btn.Text = "Deactivate";
            this.deactivate_btn.UseVisualStyleBackColor = true;
            this.deactivate_btn.Click += new System.EventHandler(this.button3_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.StatusBar.Location = new System.Drawing.Point(0, 113);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(292, 22);
            this.StatusBar.SizingGrip = false;
            this.StatusBar.TabIndex = 5;
            this.StatusBar.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // grpCache
            // 
            this.grpCache.Controls.Add(this.activationticket);
            this.grpCache.Controls.Add(this.activationdata);
            this.grpCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCache.Location = new System.Drawing.Point(12, 29);
            this.grpCache.Name = "grpCache";
            this.grpCache.Size = new System.Drawing.Size(133, 60);
            this.grpCache.TabIndex = 6;
            this.grpCache.TabStop = false;
            this.grpCache.Text = "Cache Options";
            // 
            // activationticket
            // 
            this.activationticket.AutoSize = true;
            this.activationticket.Enabled = false;
            this.activationticket.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activationticket.Location = new System.Drawing.Point(6, 39);
            this.activationticket.Name = "activationticket";
            this.activationticket.Size = new System.Drawing.Size(106, 17);
            this.activationticket.TabIndex = 2;
            this.activationticket.TabStop = false;
            this.activationticket.Text = "Activation Ticket";
            this.activationticket.UseVisualStyleBackColor = true;
            this.activationticket.CheckedChanged += new System.EventHandler(this.activationticket_CheckedChanged);
            // 
            // activationdata
            // 
            this.activationdata.AutoSize = true;
            this.activationdata.Enabled = false;
            this.activationdata.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activationdata.Location = new System.Drawing.Point(6, 19);
            this.activationdata.Name = "activationdata";
            this.activationdata.Size = new System.Drawing.Size(99, 17);
            this.activationdata.TabIndex = 1;
            this.activationdata.TabStop = false;
            this.activationdata.Text = "Activation Data";
            this.activationdata.UseVisualStyleBackColor = true;
            this.activationdata.CheckedChanged += new System.EventHandler(this.activationdata_CheckedChanged);
            // 
            // cache_btn
            // 
            this.cache_btn.Location = new System.Drawing.Point(149, 78);
            this.cache_btn.Name = "cache_btn";
            this.cache_btn.Size = new System.Drawing.Size(131, 27);
            this.cache_btn.TabIndex = 5;
            this.cache_btn.TabStop = false;
            this.cache_btn.Text = "Generate Cache";
            this.cache_btn.UseVisualStyleBackColor = true;
            this.cache_btn.Click += new System.EventHandler(this.cache_btn_Click);
            // 
            // Save
            // 
            this.Save.DefaultExt = "iticket";
            this.Save.Filter = "Activation Ticket|*.iTicket";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 135);
            this.Controls.Add(this.cache_btn);
            this.Controls.Add(this.grpCache);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.deactivate_btn);
            this.Controls.Add(this.activate_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iActivator v2.1 - iSn0wra1n";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.grpCache.ResumeLayout(false);
            this.grpCache.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button activate_btn;
        private System.Windows.Forms.Button deactivate_btn;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox grpCache;
        private System.Windows.Forms.CheckBox activationticket;
        private System.Windows.Forms.CheckBox activationdata;
        private System.Windows.Forms.Button cache_btn;
        private System.Windows.Forms.SaveFileDialog Save;
        private System.Windows.Forms.OpenFileDialog Open;
    }
}