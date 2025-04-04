
namespace Badger.Win
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.miUploadExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miGravity = new System.Windows.Forms.ToolStripMenuItem();
            this.miGameTheory = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gvCost = new System.Windows.Forms.DataGridView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpCost = new System.Windows.Forms.TabPage();
            this.tbTrip = new System.Windows.Forms.TabPage();
            this.gvTrip = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCost)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tpCost.SuspendLayout();
            this.tbTrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTrip)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(815, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLocationToolStripMenuItem,
            this.miUpload,
            this.miClose});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveLocationToolStripMenuItem
            // 
            this.saveLocationToolStripMenuItem.Name = "saveLocationToolStripMenuItem";
            this.saveLocationToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.saveLocationToolStripMenuItem.Text = "Update Save Location";
            this.saveLocationToolStripMenuItem.Click += new System.EventHandler(this.saveLocationToolStripMenuItem_Click);
            // 
            // miUpload
            // 
            this.miUpload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUploadExcel});
            this.miUpload.Name = "miUpload";
            this.miUpload.Size = new System.Drawing.Size(188, 22);
            this.miUpload.Text = "Upload File";
            // 
            // miUploadExcel
            // 
            this.miUploadExcel.Image = ((System.Drawing.Image)(resources.GetObject("miUploadExcel.Image")));
            this.miUploadExcel.Name = "miUploadExcel";
            this.miUploadExcel.Size = new System.Drawing.Size(184, 26);
            this.miUploadExcel.Text = "Excel File";
            this.miUploadExcel.Click += new System.EventHandler(this.miUploadExcel_Click);
            // 
            // miClose
            // 
            this.miClose.Name = "miClose";
            this.miClose.Size = new System.Drawing.Size(188, 22);
            this.miClose.Text = "Close";
            this.miClose.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGravity,
            this.miGameTheory});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // miGravity
            // 
            this.miGravity.Name = "miGravity";
            this.miGravity.Size = new System.Drawing.Size(197, 22);
            this.miGravity.Text = "Gravity";
            this.miGravity.Click += new System.EventHandler(this.miGravity_Click);
            // 
            // miGameTheory
            // 
            this.miGameTheory.Name = "miGameTheory";
            this.miGameTheory.Size = new System.Drawing.Size(197, 22);
            this.miGameTheory.Text = "Game Theory";
            this.miGameTheory.Click += new System.EventHandler(this.miGameTheory_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // gvCost
            // 
            this.gvCost.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvCost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvCost.Location = new System.Drawing.Point(2, 2);
            this.gvCost.Margin = new System.Windows.Forms.Padding(2);
            this.gvCost.Name = "gvCost";
            this.gvCost.RowHeadersWidth = 51;
            this.gvCost.RowTemplate.Height = 24;
            this.gvCost.Size = new System.Drawing.Size(803, 475);
            this.gvCost.TabIndex = 4;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tpCost);
            this.tabControl.Controls.Add(this.tbTrip);
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2, 28, 2, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(815, 505);
            this.tabControl.TabIndex = 5;
            // 
            // tpCost
            // 
            this.tpCost.Controls.Add(this.gvCost);
            this.tpCost.Location = new System.Drawing.Point(4, 22);
            this.tpCost.Margin = new System.Windows.Forms.Padding(2);
            this.tpCost.Name = "tpCost";
            this.tpCost.Padding = new System.Windows.Forms.Padding(2);
            this.tpCost.Size = new System.Drawing.Size(807, 479);
            this.tpCost.TabIndex = 0;
            this.tpCost.Text = "Marginal Costs Matrix";
            this.tpCost.UseVisualStyleBackColor = true;
            // 
            // tbTrip
            // 
            this.tbTrip.Controls.Add(this.gvTrip);
            this.tbTrip.Location = new System.Drawing.Point(4, 22);
            this.tbTrip.Margin = new System.Windows.Forms.Padding(2);
            this.tbTrip.Name = "tbTrip";
            this.tbTrip.Padding = new System.Windows.Forms.Padding(2);
            this.tbTrip.Size = new System.Drawing.Size(807, 479);
            this.tbTrip.TabIndex = 2;
            this.tbTrip.Text = "Trip Matrix";
            this.tbTrip.UseVisualStyleBackColor = true;
            // 
            // gvTrip
            // 
            this.gvTrip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTrip.Location = new System.Drawing.Point(2, 2);
            this.gvTrip.Margin = new System.Windows.Forms.Padding(5);
            this.gvTrip.Name = "gvTrip";
            this.gvTrip.RowHeadersWidth = 51;
            this.gvTrip.RowTemplate.Height = 24;
            this.gvTrip.Size = new System.Drawing.Size(803, 475);
            this.gvTrip.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(815, 532);
            this.panel1.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(815, 532);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Badger";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCost)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tpCost.ResumeLayout(false);
            this.tbTrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvTrip)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miGravity;
        private System.Windows.Forms.ToolStripMenuItem miUpload;
        private System.Windows.Forms.ToolStripMenuItem miUploadExcel;
        private System.Windows.Forms.ToolStripMenuItem miClose;
        private System.Windows.Forms.DataGridView gvCost;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCost;
        private System.Windows.Forms.TabPage tbTrip;
        private System.Windows.Forms.DataGridView gvTrip;
        private System.Windows.Forms.ToolStripMenuItem miGameTheory;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem saveLocationToolStripMenuItem;
    }
}

