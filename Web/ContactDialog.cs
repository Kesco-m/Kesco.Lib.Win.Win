using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Web
{
    public class ContactDialog : FreeDialog
    {
        private string baseURL;
        private string paramStr;
        private int contactID;
        private int personID;
        private string personString;
        private ExtendedBrowserControl webBrowser;
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Вызов диалога контакта с адресом, передаваемым строкой с заголовком "Добавление контакта"
        /// </summary>
        /// <param name="url">адрес для открытия</param>
        /// <param name="paramStr">строка дополнительных параметры</param>
        public ContactDialog(string url, string paramStr)
            : this(url, paramStr, "")
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ContactDialog));
            this.Text = resources.GetString("$this.Text");
        }

        /// <summary>
        /// Вызов диалога контакта с адресом, передаваемым строкой с заголовком
        /// </summary>
        /// <param name="url">адрес для открытия</param>
        /// <param name="paramStr">строка дополнительных параметры</param>
        /// <param name="titleText">строка заголовка</param>
        public ContactDialog(string url, string paramStr, string titleText)
        {
            this.DoubleBuffered = true; InitializeComponent();
            this.Text = titleText;

            if (string.IsNullOrEmpty(url))
                throw new Exception("Не задана строка запуска");
            else
                baseURL = url;

            this.paramStr = paramStr;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactDialog));
            this.webBrowser = new ExtendedBrowserControl();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.SelfNavigate = false;
            this.webBrowser.WBWantsToClose += new System.EventHandler(this.webBrowser_HandleDestroyed);
            // 
            // ContactDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.webBrowser);
            this.Name = "ContactDialog";
            this.Activated += new System.EventHandler(this.CreateContactDialog_Activated);
            this.ResumeLayout(false);

        }
        #endregion

        #region Accessers

        public int ContactID
        {
            get { return contactID; }
        }

        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public string PersonString
        {
            get { return personString; }
            set { personString = value; }
        }

        #endregion

        private void CreateContactDialog_Activated(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            webBrowser.SelfNavigate = true;
            webBrowser.Navigate(baseURL + ((baseURL.IndexOf("?") > 0) ? "&" : "?") + paramStr);
            this.Cursor = Cursors.Default;

            this.Activated -= new System.EventHandler(CreateContactDialog_Activated);
        }

        private void webBrowser_HandleDestroyed(object sender, EventArgs e)
        {
            if (webBrowser != null)
            {
                CookieCollection cc = new CookieCollection(webBrowser.Document.Cookie);
                Cookie dlgRez = cc.GetCookie("DlgRez");

                if (!(dlgRez == null || string.IsNullOrEmpty(dlgRez.Value) || dlgRez.Value.Equals("0")))
                {
                    Cookie retVal = cc.GetCookie("RetVal");
                    if ((retVal != null) && (retVal.Value != null))
                    {
                        string[] s = Regex.Split(retVal.Value, Cookie.UnitSeparator);
                        
                        if (int.TryParse(s[0], out contactID))
                            this.DialogResult = DialogResult.OK;
                    }
                }
            }
            End(this.DialogResult);
        }
    }
}

