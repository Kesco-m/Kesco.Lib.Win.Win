using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
	// Событие диалога
	public class DialogEventArgs : EventArgs
	{
	    public DialogEventArgs(Form dialog)
		{
			Dialog = dialog;
		}

		#region Accessors

	    public Form Dialog { get; private set; }

	    #endregion
	}

	// Делегат для обработки результатов работы диалогов
	public delegate void DialogEventHandler(object source, DialogEventArgs e);
}