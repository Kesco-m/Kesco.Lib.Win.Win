using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
    /// <summary>
    /// MessageBox с формой-владельцем и TextBox'ом для вывода многострочного списка
    /// </summary>
    public partial class MessageBoxScrollable : Form
    {
        private MessageBoxScrollable(string Caption, string Text, string Lines, MessageBoxButtons Buttons, MessageBoxIcon Icon, Form Owner, bool UseDontShowOption)
        {
            this.Owner = Owner;

            InitializeComponent();

            if (Owner != null)
            {
                Font = Owner.Font;
                richTextBox.Font = Owner.Font;
                label.Font = Owner.Font;
                btnOk.Font = Owner.Font;
                btnCancel.Font = Owner.Font;
                btnYes.Font = Owner.Font;
                btnNo.Font = Owner.Font;
            }

            this.Text = Caption;

            var nextLocation = new Point(10, 10);

            switch (Icon)
            {
                case MessageBoxIcon.Asterisk:
                    pictureBox.Image = SystemIcons.Asterisk.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    pictureBox.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    pictureBox.Image = SystemIcons.Question.ToBitmap();
                    break;
                case MessageBoxIcon.Warning:
                    pictureBox.Image = SystemIcons.Warning.ToBitmap();
                    break;
                default:
                    pictureBox.Image = SystemIcons.WinLogo.ToBitmap();
                    break;
            }
            pictureBox.Location = nextLocation;
            nextLocation = new Point(10 + pictureBox.Width + 10, 10);
            richTextBox.Size = new Size(Size.Width - pictureBox.Size.Width - 30, richTextBox.Font.Height * 4);


            if (string.IsNullOrEmpty(Lines))
                richTextBox.Visible = false;
            else
            {
                richTextBox.Text = Lines;
                richTextBox.Location = nextLocation;
                nextLocation = new Point(20, 10 + richTextBox.Height + 8);
            }

            if (string.IsNullOrEmpty(Text))
                label.Visible = false;
            else
            {
                label.Location = nextLocation;
                label.Text = Text;
                nextLocation = new Point(Size.Width / 2, nextLocation.Y + label.Size.Height + 8);
            }

            switch (Buttons)
            {
                case MessageBoxButtons.OK:
                    btnOk.Visible = true;
                    btnOk.Location = new Point(nextLocation.X - btnOk.Width / 2, nextLocation.Y);
					CancelButton = btnOk;
                    btnOk.Focus();
                    break;
                case MessageBoxButtons.OKCancel:
                    btnOk.Visible = true;
                    btnCancel.Visible = true;
                    btnOk.Location = new Point(nextLocation.X - btnOk.Width - 2, nextLocation.Y);
                    btnCancel.Location = new Point(nextLocation.X + 2, nextLocation.Y);
					CancelButton = btnCancel;
                    btnCancel.Focus();
                    break;
                case MessageBoxButtons.YesNo:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnYes.Location = new Point(nextLocation.X - btnYes.Width - 2, nextLocation.Y);
                    btnNo.Location = new Point(nextLocation.X + 2, nextLocation.Y);
					CancelButton = btnNo;
                    btnNo.Focus();
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnCancel.Visible = true;
                    btnYes.Location = new Point(nextLocation.X - btnYes.Width - btnNo.Width / 2 - 2, nextLocation.Y);
                    btnNo.Location = new Point(nextLocation.X - btnNo.Width / 2, nextLocation.Y);
                    btnCancel.Location = new Point(nextLocation.X + btnNo.Width / 2 + 2, nextLocation.Y);
					CancelButton = btnCancel;
                    btnCancel.Focus();
                    break;
                default:
                    btnOk.Visible = true;
                    btnOk.Location = new Point(nextLocation.X - btnOk.Width / 2, nextLocation.Y);
                    break;
            }

            if (UseDontShowOption)
            {
                dontShowOptionCheckBox.Location = new Point(20, nextLocation.Y + btnOk.Size.Height + 16);
                Height = Height - ClientSize.Height + dontShowOptionCheckBox.Location.Y + dontShowOptionCheckBox.Size.Height + 8;
            }
            else
            {
                dontShowOptionCheckBox.Visible = false;
                Height = Height - ClientSize.Height + nextLocation.Y + btnOk.Size.Height + 8;
            }
        }

        private MessageBoxScrollable(string Caption, string Text, string[] Lines, MessageBoxButtons Buttons, MessageBoxIcon Icon, Form Owner, bool UseDontShowOption)
        {
            this.Owner = Owner;

            InitializeComponent();

            if (Owner != null)
            {
                Font = Owner.Font;
                richTextBox.Font = Owner.Font;
                label.Font = Owner.Font;
                btnOk.Font = Owner.Font;
                btnCancel.Font = Owner.Font;
                btnYes.Font = Owner.Font;
                btnNo.Font = Owner.Font;
            }

            this.Text = Caption;

            var nextLocation = new Point(10, 10);

            switch (Icon)
            {
                case MessageBoxIcon.Asterisk:
                    pictureBox.Image = SystemIcons.Asterisk.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    pictureBox.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    pictureBox.Image = SystemIcons.Question.ToBitmap();
                    break;
                case MessageBoxIcon.Warning:
                    pictureBox.Image = SystemIcons.Warning.ToBitmap();
                    break;
                default:
                    pictureBox.Image = SystemIcons.WinLogo.ToBitmap();
                    break;
            }
            pictureBox.Location = nextLocation;
            nextLocation = new Point(10 + pictureBox.Width + 10, 10);
            richTextBox.Size = new Size(Size.Width - pictureBox.Size.Width - 30, richTextBox.Font.Height * 4);


            if (Lines.Length == 0)
                richTextBox.Visible = false;
            else
            {
                richTextBox.Lines = Lines;
                richTextBox.Location = nextLocation;
                nextLocation = new Point(20, 10 + richTextBox.Height + 10);
            }

            if (string.IsNullOrEmpty(Text))
                label.Visible = false;
            else
            {
                label.Location = nextLocation;
                label.Text = Text;
                nextLocation = new Point(Size.Width / 2, nextLocation.Y + label.Size.Height + 10);
            }

            switch (Buttons)
            {
                case MessageBoxButtons.OK:
                    btnOk.Visible = true;
                    btnOk.Location = new Point(nextLocation.X - btnOk.Width / 2, nextLocation.Y);
					CancelButton = btnOk;
                    btnOk.Focus();
                    break;
                case MessageBoxButtons.OKCancel:
                    btnOk.Visible = true;
                    btnCancel.Visible = true;
                    btnOk.Location = new Point(nextLocation.X - btnOk.Width - 2, nextLocation.Y);
                    btnCancel.Location = new Point(nextLocation.X + 2, nextLocation.Y);
					CancelButton = btnCancel;
                    btnCancel.Focus();
                    break;
                case MessageBoxButtons.YesNo:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnYes.Location = new Point(nextLocation.X - btnYes.Width - 2, nextLocation.Y);
                    btnNo.Location = new Point(nextLocation.X + 2, nextLocation.Y);
					CancelButton = btnNo;
                    btnNo.Focus();
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnCancel.Visible = true;
                    btnYes.Location = new Point(nextLocation.X - btnYes.Width - btnNo.Width / 2 - 2, nextLocation.Y);
                    btnNo.Location = new Point(nextLocation.X - btnNo.Width / 2, nextLocation.Y);
                    btnCancel.Location = new Point(nextLocation.X + btnNo.Width / 2 + 2, nextLocation.Y);
					CancelButton = btnCancel;
                    btnCancel.Focus();
                    break;
                default:
                    btnOk.Visible = true;
                    btnOk.Location = new Point(nextLocation.X - btnOk.Width / 2, nextLocation.Y);
                    break;
            }

            if (UseDontShowOption)
            {
                dontShowOptionCheckBox.Location = new Point(20, nextLocation.Y + btnOk.Size.Height + 20);
                Height = Height - ClientSize.Height + dontShowOptionCheckBox.Location.Y + dontShowOptionCheckBox.Size.Height + 10;
            }
            else
            {
                dontShowOptionCheckBox.Visible = false;
                Height = Height - ClientSize.Height + nextLocation.Y + btnOk.Size.Height + 10;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Owner = null;
            }
            catch { }
            finally
            {
                Close();
            }
        }

        private void MessageBoxScrollable_Shown(object sender, EventArgs e)
        {
            if (btnCancel.Visible)
                btnCancel.Focus();
            else if (btnNo.Visible)
                btnNo.Focus();
            else if (btnOk.Visible)
                btnOk.Focus();
        }

        public static DialogResult Show(string Caption, string Text, string Lines, MessageBoxButtons Buttons, MessageBoxIcon Icon, Form Owner, out bool DontShowOption, out bool NeedSaveOption)
        {
            DontShowOption = false;
            NeedSaveOption = false;
            try
            {
                var msgBox = new MessageBoxScrollable(Caption, Text, Lines, Buttons, Icon, Owner, true);
                DialogResult result = msgBox.ShowDialog();

                switch (result)
                {
                    case DialogResult.Cancel:
                    case DialogResult.None:
                        break;
                    default:
                        DontShowOption = msgBox.dontShowOptionCheckBox.Checked;
                        NeedSaveOption = true;
                        break;
                }

                return result;
            }
            catch
            {
                return DialogResult.None;
            }
        }

        public static DialogResult Show(string Caption, string Text, string[] Lines, MessageBoxButtons Buttons, MessageBoxIcon Icon, Form Owner, out bool DontShowOption, out bool NeedSaveOption)
        {
            DontShowOption = false;
            NeedSaveOption = false;
            try
            {
                var msgBox = new MessageBoxScrollable(Caption, Text, Lines, Buttons, Icon, Owner, true);
                DialogResult result = msgBox.ShowDialog();

                switch (result)
                {
                    case DialogResult.Cancel:
                    case DialogResult.None:
                        break;
                    default:
                        DontShowOption = msgBox.dontShowOptionCheckBox.Checked;
                        NeedSaveOption = true;
                        break;
                }

                return result;
            }
            catch
            {
                return DialogResult.None;
            }
        }
    }
}
