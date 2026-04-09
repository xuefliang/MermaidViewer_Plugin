namespace MermaidViewer.Forms
{
    partial class SettingsForm
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
            this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
            this.chkDarkMode = new System.Windows.Forms.CheckBox();
            this.numRefreshDelay = new System.Windows.Forms.NumericUpDown();
            this.numPngScale = new System.Windows.Forms.NumericUpDown();
            this.txtExportPath = new System.Windows.Forms.TextBox();
            this.txtMmdrPath = new System.Windows.Forms.TextBox();
            this.chkFollowDarkMode = new System.Windows.Forms.CheckBox();
            this.btnBrowseMmdr = new System.Windows.Forms.Button();
            this.btnBrowseExport = new System.Windows.Forms.Button();
            this.btnTestMmdr = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRefreshDelay = new System.Windows.Forms.Label();
            this.lblPngScale = new System.Windows.Forms.Label();
            this.lblExportPath = new System.Windows.Forms.Label();
            this.lblMmdrPath = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numRefreshDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPngScale)).BeginInit();
            this.SuspendLayout();

            // chkAutoRefresh
            this.chkAutoRefresh.AutoSize = true;
            this.chkAutoRefresh.Location = new System.Drawing.Point(12, 12);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.Size = new System.Drawing.Size(120, 17);
            this.chkAutoRefresh.TabIndex = 0;
            this.chkAutoRefresh.Text = "Auto Refresh";
            this.chkAutoRefresh.UseVisualStyleBackColor = true;

            // chkDarkMode
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Location = new System.Drawing.Point(12, 35);
            this.chkDarkMode.Name = "chkDarkMode";
            this.chkDarkMode.Size = new System.Drawing.Size(120, 17);
            this.chkDarkMode.TabIndex = 1;
            this.chkDarkMode.Text = "Dark Mode";
            this.chkDarkMode.UseVisualStyleBackColor = true;

            // chkFollowDarkMode
            this.chkFollowDarkMode.AutoSize = true;
            this.chkFollowDarkMode.Location = new System.Drawing.Point(12, 58);
            this.chkFollowDarkMode.Name = "chkFollowDarkMode";
            this.chkFollowDarkMode.Size = new System.Drawing.Size(180, 17);
            this.chkFollowDarkMode.TabIndex = 2;
            this.chkFollowDarkMode.Text = "Follow Notepad++ Dark Mode";
            this.chkFollowDarkMode.UseVisualStyleBackColor = true;

            // lblRefreshDelay
            this.lblRefreshDelay.AutoSize = true;
            this.lblRefreshDelay.Location = new System.Drawing.Point(12, 85);
            this.lblRefreshDelay.Name = "lblRefreshDelay";
            this.lblRefreshDelay.Size = new System.Drawing.Size(100, 13);
            this.lblRefreshDelay.TabIndex = 3;
            this.lblRefreshDelay.Text = "Refresh Delay (ms):";

            // numRefreshDelay
            this.numRefreshDelay.Location = new System.Drawing.Point(120, 83);
            this.numRefreshDelay.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            this.numRefreshDelay.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numRefreshDelay.Name = "numRefreshDelay";
            this.numRefreshDelay.Size = new System.Drawing.Size(150, 20);
            this.numRefreshDelay.TabIndex = 4;
            this.numRefreshDelay.Value = new decimal(new int[] { 500, 0, 0, 0 });

            // lblPngScale
            this.lblPngScale.AutoSize = true;
            this.lblPngScale.Location = new System.Drawing.Point(12, 110);
            this.lblPngScale.Name = "lblPngScale";
            this.lblPngScale.Size = new System.Drawing.Size(100, 13);
            this.lblPngScale.TabIndex = 5;
            this.lblPngScale.Text = "PNG Scale:";

            // numPngScale
            this.numPngScale.DecimalPlaces = 2;
            this.numPngScale.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            this.numPngScale.Location = new System.Drawing.Point(120, 108);
            this.numPngScale.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numPngScale.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            this.numPngScale.Name = "numPngScale";
            this.numPngScale.Size = new System.Drawing.Size(150, 20);
            this.numPngScale.TabIndex = 6;
            this.numPngScale.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblExportPath
            this.lblExportPath.AutoSize = true;
            this.lblExportPath.Location = new System.Drawing.Point(12, 135);
            this.lblExportPath.Name = "lblExportPath";
            this.lblExportPath.Size = new System.Drawing.Size(100, 13);
            this.lblExportPath.TabIndex = 7;
            this.lblExportPath.Text = "Default Export Path:";

            // txtExportPath
            this.txtExportPath.Location = new System.Drawing.Point(120, 133);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Size = new System.Drawing.Size(280, 20);
            this.txtExportPath.TabIndex = 8;

            // btnBrowseExport
            this.btnBrowseExport.Location = new System.Drawing.Point(410, 133);
            this.btnBrowseExport.Name = "btnBrowseExport";
            this.btnBrowseExport.Size = new System.Drawing.Size(40, 20);
            this.btnBrowseExport.TabIndex = 9;
            this.btnBrowseExport.Text = "...";
            this.btnBrowseExport.UseVisualStyleBackColor = true;
            this.btnBrowseExport.Click += new System.EventHandler(this.btnBrowseExport_Click);

            // lblMmdrPath
            this.lblMmdrPath.AutoSize = true;
            this.lblMmdrPath.Location = new System.Drawing.Point(12, 160);
            this.lblMmdrPath.Name = "lblMmdrPath";
            this.lblMmdrPath.Size = new System.Drawing.Size(100, 13);
            this.lblMmdrPath.TabIndex = 10;
            this.lblMmdrPath.Text = "mmdr.exe Path:";

            // txtMmdrPath
            this.txtMmdrPath.Location = new System.Drawing.Point(120, 158);
            this.txtMmdrPath.Name = "txtMmdrPath";
            this.txtMmdrPath.Size = new System.Drawing.Size(280, 20);
            this.txtMmdrPath.TabIndex = 11;

            // btnBrowseMmdr
            this.btnBrowseMmdr.Location = new System.Drawing.Point(410, 158);
            this.btnBrowseMmdr.Name = "btnBrowseMmdr";
            this.btnBrowseMmdr.Size = new System.Drawing.Size(40, 20);
            this.btnBrowseMmdr.TabIndex = 12;
            this.btnBrowseMmdr.Text = "...";
            this.btnBrowseMmdr.UseVisualStyleBackColor = true;
            this.btnBrowseMmdr.Click += new System.EventHandler(this.btnBrowseMmdr_Click);

            // btnTestMmdr
            this.btnTestMmdr.Location = new System.Drawing.Point(410, 180);
            this.btnTestMmdr.Name = "btnTestMmdr";
            this.btnTestMmdr.Size = new System.Drawing.Size(40, 20);
            this.btnTestMmdr.TabIndex = 13;
            this.btnTestMmdr.Text = "Test";
            this.btnTestMmdr.UseVisualStyleBackColor = true;
            this.btnTestMmdr.Click += new System.EventHandler(this.btnTestMmdr_Click);

            // btnOK
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(350, 220);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(50, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(406, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // SettingsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 255);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTestMmdr);
            this.Controls.Add(this.btnBrowseMmdr);
            this.Controls.Add(this.txtMmdrPath);
            this.Controls.Add(this.lblMmdrPath);
            this.Controls.Add(this.btnBrowseExport);
            this.Controls.Add(this.txtExportPath);
            this.Controls.Add(this.lblExportPath);
            this.Controls.Add(this.numPngScale);
            this.Controls.Add(this.lblPngScale);
            this.Controls.Add(this.numRefreshDelay);
            this.Controls.Add(this.lblRefreshDelay);
            this.Controls.Add(this.chkFollowDarkMode);
            this.Controls.Add(this.chkDarkMode);
            this.Controls.Add(this.chkAutoRefresh);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mermaid Viewer Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numRefreshDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPngScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox chkAutoRefresh;
        private System.Windows.Forms.CheckBox chkDarkMode;
        private System.Windows.Forms.NumericUpDown numRefreshDelay;
        private System.Windows.Forms.NumericUpDown numPngScale;
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.TextBox txtMmdrPath;
        private System.Windows.Forms.CheckBox chkFollowDarkMode;
        private System.Windows.Forms.Button btnBrowseMmdr;
        private System.Windows.Forms.Button btnBrowseExport;
        private System.Windows.Forms.Button btnTestMmdr;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRefreshDelay;
        private System.Windows.Forms.Label lblPngScale;
        private System.Windows.Forms.Label lblExportPath;
        private System.Windows.Forms.Label lblMmdrPath;
    }
}
