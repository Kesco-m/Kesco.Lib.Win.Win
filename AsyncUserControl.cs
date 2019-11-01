using System.Threading;
using System.Windows.Forms;

namespace Kesco.Lib.Win
{
	public partial class AsyncUserControl : UserControl
	{
		protected internal CancellationTokenSource source = new CancellationTokenSource();
		public AsyncUserControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Отмена асинхронных задач.
		/// </summary>
		public virtual void CancelOperation()
		{
			source.Cancel();
			source.Dispose();
			source = new CancellationTokenSource();
		}
	}
}
