using System.Windows.Forms;

namespace Kesco.Lib.Win.Trees
{
	/// <summary>
	/// Базовый тип узла дерева с добавкой поля id
	/// </summary>
	public class DTreeNode : TreeNode
	{
		protected int id;
		protected bool isNew;
		protected bool isGag; //Заглушка (для !FullLoad)
		
		public DTreeNode()
		{
		}

		public DTreeNode(int id, string text) : base(text)
		{
			this.id = id;
		}

		#region Accessors

		public int ID
		{
			get { return id; }
			set { id = value; }
		}
		public bool IsNew
		{
			get { return isNew; }
			set { isNew = value; }
		}
		public bool IsGag
		{
			get { return isGag; }
			set { isGag = value; }
		}

		#endregion
	}
}