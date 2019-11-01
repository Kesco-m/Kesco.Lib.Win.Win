using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kesco.Lib.Win.EmployeeDialog
{
    public class PhoneList : UserControl
    {
        private Panel panel1;

        private Container components;

        public PhoneList()
        {
            DoubleBuffered = true; InitializeComponent();
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

        public void AddPhones(string[] phones)
        {
            if (phones == null || phones.Length <= 0)
            {
                Visible = false;
                return;
            }
            
            Visible = true;
            panel1.Controls.Clear();
            int x = 0;
            int y = 0;
            foreach (string phone in phones)
            {
                var l = new Label
                            {
                                AutoSize = true,
                                Text = panel1.Controls.Count == 0 ? "Тел.: " + phone + ", " : phone + ", "
                            };
                if ((x + l.Width) > panel1.Width)
                {
                    x = 0;
                    y += l.Height + 4;
                    l.Location = new Point(x, y);
                }
                else
                {
                    l.Location = new Point(x, y);
                    x += l.Width;
                }
                panel1.Controls.Add(l);
            }
            var lastLabel = ((Label)panel1.Controls[panel1.Controls.Count - 1]);
            lastLabel.Text = lastLabel.Text.Remove(lastLabel.Text.Length - 2, 2);
            panel1.Height = lastLabel.Bottom + 4;
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 136);
            this.panel1.TabIndex = 0;
            // 
            // PhoneList
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "PhoneList";
            this.Size = new System.Drawing.Size(272, 136);
            this.ResumeLayout(false);

        }
        #endregion
    }
}