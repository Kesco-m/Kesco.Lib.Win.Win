using System;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
	// ������� �������
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

	// ������� ��� ��������� ����������� ������ ��������
	public delegate void DialogEventHandler(object source, DialogEventArgs e);
}