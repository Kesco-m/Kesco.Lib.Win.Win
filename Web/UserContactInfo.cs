using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
	public class UserContactInfo : FreeDialog
	{
        private string Url;
        private CookieCollection cc;
        private ExtendedBrowserControl webBrowser;
		private System.ComponentModel.IContainer components = null;

		public UserContactInfo(string URL, string labelText):base()
		{
			this.DoubleBuffered = true; InitializeComponent();
			this.Text = labelText;
            Url = URL;
			this.webBrowser.WBWantsToClose += new EventHandler(webBrowser_HandleDestroyed);
            this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted); 
		}


		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.webBrowser = new ExtendedBrowserControl();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(752, 430);
            this.webBrowser.TabIndex = 0;
            // 
            // UserContactInfo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(752, 430);
            this.Controls.Add(this.webBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UserContactInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.UserContactInfo_Load);
            this.ResumeLayout(false);

		}
		#endregion

		#region Accessers

		public CookieCollection Collection
		{
			get { return cc;}
		}

		#endregion

		private void UserContactInfo_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(Url) && Url != "about:blank")
			{
				webBrowser.SelfNavigate = true;
				webBrowser.Url = new Uri(Url);
			}
		}

		private void webBrowser_HandleDestroyed(object sender, EventArgs e)
		{
			this.webBrowser.WBWantsToClose -= new System.EventHandler(webBrowser_HandleDestroyed);

            if (webBrowser != null && webBrowser.Document != null)
			{
                string cookie = webBrowser.Document.Cookie;
                cc = new CookieCollection(cookie);

				Cookie dlgRez = cc.GetCookie("DlgRez");

				if ((dlgRez != null) && (dlgRez.Value != null) && (dlgRez.Value != "0"))
				{
					this.DialogResult = DialogResult.OK;
				}
			}
			End( this.DialogResult);
		}

        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //	webBrowser.Document.Body.Styleborder = "1px solid";
            HtmlElementCollection ec = webBrowser.Document.GetElementsByTagName("a");
            foreach (HtmlElement elem in ec)
            {
                //elem.Style.Contains("fontFamily")
                //elem.Style.Replace( = "Microsoft Sans Serif";
                //elem.Style.fontSize = "8.25pt";
            }
        }

	}
}