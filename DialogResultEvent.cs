using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
	/// <summary>
	/// Summary description for DialogResultEvent.
	/// </summary>
	public class DialogResultEvent : EventArgs
	{
	    public DialogResultEvent(DialogResult result, object[] param)
		{
			Result = result;
			Params = param;
		}

		#region Accessors

	    public DialogResult Result { get; private set; }

	    public object[] Params { get; private set; }

	    #endregion
	}

	/// <summary>
	/// Делегат для обработки результатов работы диалогов
	/// </summary>
 	public delegate void DialogResultHandler(object source, DialogResultEvent e);
}
