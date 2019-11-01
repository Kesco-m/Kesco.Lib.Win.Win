using System;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Kesco.Lib.Win.Web
{
	/// <summary>
	/// Summary description for PersonDialog.
	/// </summary>
	public class PersonDialog : FreeDialog
	{
		private string baseURL;
		private string paramStr;
		private ArrayList persons;
        private ExtendedBrowserControl browser;

		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;

		/// <summary>
		/// ¬ызов диалога поиска лиц с адресом, передаваемым строкой
		/// </summary>
		/// <param name="url">адрес дл€ открыти€</param>
		/// <param name="paramStr">строка дополнительных параметры</param>
		public PersonDialog( string url, string paramStr)
		{
			this.DoubleBuffered = true; InitializeComponent();


			if(url ==null || url == "")
				throw new Exception("Ќе задана строка запуска");
			else
				baseURL = url;

			persons = new ArrayList();
			this.paramStr = paramStr;
		}

		#region Accessors

		public string BaseURL
		{
			get { return baseURL; }
		}

		public string ParamStr
		{
			get { return paramStr; }
		}

		public ArrayList Persons
		{
			get { return persons; }
		}

		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonDialog));
            this.browser = new ExtendedBrowserControl();
            this.SuspendLayout();
            // 
            // browser
            // 
            resources.ApplyResources(this.browser, "browser");
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.WBWantsToClose += new EventHandler(BrowserClose);
            this.browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.browser_DocumentCompleted);
            // 
            // PersonDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.browser);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonDialog";
            this.Activated += new System.EventHandler(this.PersonDialog_Activated);
            this.ResumeLayout(false);

		}
		#endregion

		private void PersonDialog_Activated(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			//LoadBrowser();
			browser.SelfNavigate = true;
            //browser.Url = new Uri(baseURL + ((baseURL.IndexOf("?")>0)?"&":"?") + paramStr);
            browser.Navigate(baseURL + ((baseURL.IndexOf("?") > 0) ? "&" : "?") + paramStr);

			this.Cursor = Cursors.Default;

			this.Activated -= new System.EventHandler(PersonDialog_Activated);
		}

		private void BrowserClose(object sender, System.EventArgs e)
		{
			CookieCollection cc;

			if (browser != null)
			{
                if (browser.Document != null)
                {
                    string cookie = browser.Document.Cookie;
                    cc = new CookieCollection(cookie);

                    Cookie dlgRez = cc.GetCookie("DlgRez");

                    if ((dlgRez != null) && (dlgRez.Value != null) && (dlgRez.Value != "0"))
                    {
                        Cookie retVal = cc.GetCookie("RetVal");

                        if ((retVal != null) && (retVal.Value != null))
                        {
                            string retStr = retVal.Value;
                            string rSep = Cookie.RecordSeparator;
                            string uSep = Cookie.UnitSeparator;

                            Regex r = new Regex("(?<id>((?!" + uSep + ").)+)" + uSep + "(?<name>((?!" + rSep + ").)+)(" + rSep + "|$)", RegexOptions.IgnoreCase);
                            Match m = r.Match(retStr);

                            while (m.Success)
                            {
                                try
                                {
                                    PersonInfo person = new PersonInfo();
                                    person.ID = Int32.Parse(m.Groups["id"].Value);
                                    person.Name = System.Web.HttpUtility.UrlDecode (m.Groups["name"].Value);

                                    persons.Add(person);
                                }
                                catch { }

                                m = m.NextMatch();
                            }

                            if (persons.Count > 0)
                                this.DialogResult = DialogResult.OK;
                        }
                    }
                }
			}
			End(this.DialogResult);
		}

	    private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.browser.Disposed -= new EventHandler(BrowserClose);
            this.browser.Disposed += new EventHandler(BrowserClose);
        }
	}

	public class PersonInfo
	{
		public int ID;
		public string Name;
	}
}