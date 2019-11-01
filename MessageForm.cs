using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Kesco.Lib.Win.Error;

namespace Kesco.Lib.Win
{
	/// <summary>
	/// Summary description for MessageForm.
	/// </summary>
	public class MessageForm : FreeDialog
	{
		private Label textLabel;
		private Button button1;
		private Button button2;

	    /// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components;

		public MessageForm(string messageText, string controlText, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
		{
			ErrorShower.StartErrorShow += ErrorShower_StartErrorShow;
			ErrorShower.ErrorShowEnd += ErrorShower_ErrorShowEnd;

		    int stringCount=0;

            InitializeComponent();

			if (messageText!=null)
			{
				textLabel.Text = messageText;
				stringCount = Regex.Matches(messageText,"[\n]").Count;
			}

			if (controlText!=null)
			{
				Text = controlText;
			}

			switch(buttons)
			{
				case MessageBoxButtons.YesNo :
					button1 = new Button();
					button2 = new Button();
					// 
					// button1
					// 
					SuspendLayout();
					button1.Anchor = AnchorStyles.Bottom;
					button1.DialogResult = DialogResult.Yes;
					button1.Location = new Point(51, 32);
					button1.Name = "button1";
					button1.TabIndex = 1;
					button1.Text = Thread.CurrentThread.CurrentUICulture.Name.StartsWith("ru") ? "Да" : "Yes";
					button1.Click +=button1_Click;
					//	this.button1.Show();
					if (defaultButton == MessageBoxDefaultButton.Button1)
						button1.TabIndex = 0;
					// 
					// button2
					// 
					button2.Anchor = AnchorStyles.Bottom;
					button2.DialogResult = DialogResult.No;
					button2.Location = new Point(168, 32);
					button2.Name = "button2";
					button2.TabIndex = 2;
					button2.Text = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.Equals("ru") ? "Нет" : "No";
					button2.Click +=button2_Click;
								//		this.button2.Show();
					Controls.Add(button1);
					Controls.Add(button2);
					this.CancelButton = button2;
					if (defaultButton == MessageBoxDefaultButton.Button2)
						button2.TabIndex = 0;
					ResumeLayout(false);
					break;

				default:
					button1 = new Button();
					SuspendLayout();
					button1.Anchor = AnchorStyles.Bottom;
					button1.Location = new Point(115, 32);
					button1.Name = "button1";
					button1.TabIndex = 0;
					button1.DialogResult = DialogResult.OK;
					button1.Click +=button1_Click;
					button1.Text = "OK";
					AcceptButton = button1;
					CancelButton = button1;
					Controls.Add(button1);
					ResumeLayout(false);
					break;
			}
			if(ErrorShower.ErrorShown)
				TopMost = false;
			textLabel.AutoSize = true;
			Size formSize = textLabel.Size;
			textLabel.AutoSize = false;
			textLabel.Size = new Size(227, 16);
			ClientSize = new Size(formSize.Width+56,(formSize.Height+stringCount*(int)textLabel.Font.GetHeight())+47);
		}

        public MessageForm(string messageText, string controlText, MessageBoxButtons buttons, int docID, int empID)
            : this(messageText, controlText, buttons, MessageBoxDefaultButton.Button1)
        {
            DocID = docID;
            EmpID = empID;
        }

        public MessageForm(string messageText, string controlText, MessageBoxButtons buttons, int docID)
            : this(messageText, controlText, buttons, MessageBoxDefaultButton.Button1)
        {
            DocID = docID;
        }

		public MessageForm(string messageText, string controlText, MessageBoxButtons buttons) : this(messageText, controlText, buttons, MessageBoxDefaultButton.Button1)
		{
		}

		public MessageForm(string messageText, string controlText) : this(messageText, controlText,  MessageBoxButtons.OK, MessageBoxDefaultButton.Button1)
		{
		}

		public MessageForm(string messageText) : this(messageText, "",  MessageBoxButtons.OK, MessageBoxDefaultButton.Button1)
		{
		}

		#region Accessors

	    public int DocID { get; private set; }

	    public int EmpID { get; private set; }

	    #endregion

		// Show methods
		public static void Show(string messageText, string controlText, MessageBoxButtons buttons, int docID)
		{
			var form = new MessageForm(messageText, controlText, buttons, docID);
			form.Show();
		}

		public static void Show(string messageText, string controlText, MessageBoxButtons buttons)
		{
			var form = new MessageForm(messageText, controlText, buttons);
			form.Show();
		}

		public static void Show(string messageText, string controlText)
		{
			var form = new MessageForm(messageText, controlText);
			form.Show();
		}

		public static void Show(string messageText)
		{
			var form = new MessageForm(messageText);
			form.Show();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			ErrorShower.StartErrorShow -= ErrorShower_StartErrorShow;
			ErrorShower.ErrorShowEnd -= ErrorShower_ErrorShowEnd;
			
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			var resources = new System.Resources.ResourceManager(typeof(MessageForm));
			this.textLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textLabel
			// 
			this.textLabel.AccessibleDescription = resources.GetString("textLabel.AccessibleDescription");
			this.textLabel.AccessibleName = resources.GetString("textLabel.AccessibleName");
			this.textLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("textLabel.Anchor")));
			this.textLabel.AutoSize = ((bool)(resources.GetObject("textLabel.AutoSize")));
			this.textLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("textLabel.Dock")));
			this.textLabel.Enabled = ((bool)(resources.GetObject("textLabel.Enabled")));
			this.textLabel.Font = ((System.Drawing.Font)(resources.GetObject("textLabel.Font")));
			this.textLabel.Image = ((System.Drawing.Image)(resources.GetObject("textLabel.Image")));
			this.textLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("textLabel.ImageAlign")));
			this.textLabel.ImageIndex = ((int)(resources.GetObject("textLabel.ImageIndex")));
			this.textLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("textLabel.ImeMode")));
			this.textLabel.Location = ((System.Drawing.Point)(resources.GetObject("textLabel.Location")));
			this.textLabel.Name = "textLabel";
			this.textLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("textLabel.RightToLeft")));
			this.textLabel.Size = ((System.Drawing.Size)(resources.GetObject("textLabel.Size")));
			this.textLabel.TabIndex = ((int)(resources.GetObject("textLabel.TabIndex")));
			this.textLabel.Text = resources.GetString("textLabel.Text");
			this.textLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("textLabel.TextAlign")));
			this.textLabel.Visible = ((bool)(resources.GetObject("textLabel.Visible")));
			// 
			// MessageForm
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.textLabel);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimizeBox = false;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "MessageForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.TopMost = true;
			this.StyleChanged += new System.EventHandler(this.MessageForm_StyleChanged);
			this.Activated += new System.EventHandler(this.MessageForm_Activated);
			this.ResumeLayout(false);

		}
		#endregion

	    private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MessageForm_StyleChanged(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Normal;
		}

		private void MessageForm_Activated(object sender, EventArgs e)
		{
			if(!ErrorShower.ErrorShown)
				TopMost = true;
		}

		private void ErrorShower_StartErrorShow(object sender, EventArgs e)
		{
			TopMost = false;
		}

		private void ErrorShower_ErrorShowEnd(object sender, EventArgs e)
		{
			TopMost = true;
		}
	}
}