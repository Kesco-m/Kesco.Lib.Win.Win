using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
	/// <summary>
	/// Summary description for FreeDialog.
	/// </summary>
	public class FreeDialog : Form
	{
		public event DialogEventHandler DialogEvent;
		public event DialogResultHandler ResultEvent;

		public FreeDialog() : this(null)
		{			
		}

		public FreeDialog(DialogEventHandler dialogEvent)
		{
            InitializeComponent();

			if (dialogEvent != null)
				DialogEvent += dialogEvent;

			Activated += On_Activated;
			Closed += On_Closed;
		}

		protected virtual void End(DialogResult result)
		{
			DialogResult = result;
			Owner = null;
			Close();
		}

		protected virtual void EndWithParent(DialogResult result)
		{
			DialogResult = result;
			if (Parent != null)
				Parent = null;
			Close();
			if (Owner != null)
				Owner = null;
		}

		protected virtual void End(DialogResult result, object [] param)
		{
			DialogResult = result;
			if(ResultEvent != null)
				ResultEvent.DynamicInvoke (new object[] {this, new DialogResultEvent( result, param)});
			Close();
		}

		private void On_Closed(object sender, EventArgs e)
		{
			try
			{
				if (DialogEvent != null)
					DialogEvent(this, new DialogEventArgs(this));
			}
			catch(Exception ex)
			{
                MessageBox.Show(ex.ToString());
			}
		}

		public void ShowSubForm(Form sub)
		{
			if (sub != null)
			{
				sub.Owner = this;
				sub.Show();
			}
		}

		private void On_Activated(object sender, EventArgs e)
		{
			if (!Enabled && OwnedForms.Length > 0)
			{
				Form lastForm = this;
				for (int i = 0; i < OwnedForms.Length; i++)
				{
					lastForm = OwnedForms[i];
					if (OwnedForms[i].Enabled)
						break;
				}

				lastForm.Activate();
			}
		}

		public void InvokeIfRequired( MethodInvoker action)
		{
			if(InvokeRequired)
			{
				Invoke(action);
			}
			else
			{
				action();
			}
		}

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // FreeDialog
            // 
            ClientSize = new Size(284, 262);
            DoubleBuffered = true;
            Name = "FreeDialog";
            ResumeLayout(false);
        }
	}
}