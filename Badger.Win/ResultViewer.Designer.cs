
namespace Badger.Win
{
    partial class ResultViewer
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
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(667, 419);
            this.txtResult.TabIndex = 1;
            this.txtResult.Text = "";
            // 
            // ResultViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 419);
            this.Controls.Add(this.txtResult);
            this.Name = "ResultViewer";
            this.Text = "ResultViewer";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox txtResult;
    }
}