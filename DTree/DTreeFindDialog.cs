using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Trees
{
    internal class DTreeFindDialog : Form
    {
        private DTree tree;
        private TextBox txtFind;
        private Button buttonFind;
        private Label lblFind;
        private Container components;

        public DTreeFindDialog(DTree tree)
        {
            InitializeComponent();

            this.tree = tree;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.buttonFind = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFind
            // 
            this.lblFind.Location = new System.Drawing.Point(11, 21);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(40, 16);
            this.lblFind.TabIndex = 3;
            this.lblFind.Text = "Найти:";
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(56, 17);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(128, 20);
            this.txtFind.TabIndex = 1;
            this.txtFind.TextChanged += new System.EventHandler(this.txtFind_TextChanged);
            this.txtFind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFind_KeyUp);
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(192, 15);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(80, 23);
            this.buttonFind.TabIndex = 2;
            this.buttonFind.Text = "Найти далее";
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // DTreeFindDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(282, 55);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.lblFind);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DTreeFindDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Поиск";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void txtFind_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (buttonFind.Enabled)
                        buttonFind_Click(sender, e);
                    break;

                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            buttonFind.Enabled = false;
            buttonFind.Enabled = (tree.FindNext(txtFind.Text));
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            buttonFind.Enabled = true;
        }
    }
}
