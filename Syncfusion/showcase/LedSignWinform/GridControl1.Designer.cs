using Syncfusion.Windows.Forms.Grid;
using System.Drawing;
using System.Windows.Forms;
namespace LedSignWinform
{
    partial class GridControl1
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
            SuspendLayout();
            // 
            // GridControl1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1028, 540);
            Location = new Point(170, 10);
            Margin = new Padding(3, 4, 3, 4);
            Name = "GridControl1";
            Text = "GridControl1";
            Load += GridControl1_Load;
            ResumeLayout(false);
        }
        #endregion
        private Syncfusion.Windows.Forms.Grid.GridControl gridControl1;
    }
}
