using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Kesco.Lib.Win.Trees
{
    /// <summary>
    /// Элемент управления дерево!
    /// </summary>
    public class DTree : TreeView
    {
        private string connectionString;

        private string tableName;
        private string queryString;

        private string querySelect;
        private string queryJoin;
        private string queryWhere;
        private string queryOrderBy;

        protected string idField;
        protected string textField;
        protected string parentField = "Parent";
        protected string leftField = "L";
        protected string rightField = "R";
        protected Type nodeType = typeof (DTreeNode);
        protected DataSet buffer;

        protected bool fullLoad;

        private bool allowAdd = true;
        private bool allowEdit = true;
        private bool allowDelete = true;
        private bool allowMove = true;
        private bool allowFind = true;

        private Color colorMove = Color.CornflowerBlue;
        private Color colorInsert = Color.SkyBlue;

        private System.Threading.Timer timer;
        private TreeNode dond;

        private string searchString = "";
        private DTreeFindDialog findDialog;

        private ContextMenu contextMenu;

        private MenuItem renameItem;
        private MenuItem newItem;
        private MenuItem deleteItem;
        private MenuItem deleteAllItem;
        private MenuItem sortItem;
        private MenuItem findItem;
        private MenuItem refreshItem;

        private MenuItem separator1;
        private MenuItem separator2;

        public event FillNodeEventHandler FillNode;
        public event TreeViewCancelEventHandler AfterNewNodeEdit;
        public event CheckNodeForDropEventHandler CheckNodeForDrop;
        public event RefreshNodeDelegate RefreshNodeEvent;
        
        protected void OnAfterNewNodeEdit(TreeViewCancelEventArgs e)
        {
            if (AfterNewNodeEdit != null) AfterNewNodeEdit(this, e);
        }

        public void OnRefreshNode()
        {
            if (RefreshNodeEvent != null)
                RefreshNodeEvent();
        }

        #region Constructor

        public DTree()
        {
            DoubleBuffered = true;
            timer = new System.Threading.Timer(ExpandNode, null, -1, -1);

            HideSelection = false;

            // контекстное меню
            contextMenu = new ContextMenu();
            ContextMenu = contextMenu;


            // пункты меню
            newItem = new MenuItem();
            renameItem = new MenuItem();
            deleteItem = new MenuItem();
            deleteAllItem = new MenuItem();
            sortItem = new MenuItem();
            findItem = new MenuItem();
            refreshItem = new MenuItem();

            separator1 = new MenuItem();
            separator2 = new MenuItem();

            // добавляем пункты в меню
            contextMenu.MenuItems.AddRange(new[]
                                               {
                                                   newItem,
                                                   renameItem,
                                                   deleteItem,
                                                   deleteAllItem,
                                                   separator1,
                                                   refreshItem,
                                                   separator2,
                                                   findItem,
                                                   sortItem
                                               });

            // добавить
            newItem.Index = 0;
            newItem.Text = "Добавить";
            newItem.Shortcut = Shortcut.Ins;
            newItem.Enabled = allowAdd;
            newItem.Click += newItem_Click;

            // переименовать
            renameItem.Index = 1;
            renameItem.Text = "Переименовать";
            renameItem.Shortcut = Shortcut.F2;
            renameItem.Enabled = allowEdit;
            renameItem.Click += renameItem_Click;

            // удалить
            deleteItem.Index = 2;
            deleteItem.Text = "Удалить узел";
            deleteItem.Shortcut = Shortcut.Del;
            deleteItem.Enabled = allowDelete;
            deleteItem.Click += deleteItem_Click;

            // удалить поддерево
            deleteAllItem.Index = 3;
            deleteAllItem.Text = "Удалить поддерево";
            deleteAllItem.Shortcut = Shortcut.CtrlDel;
            deleteAllItem.Enabled = allowDelete;
            deleteAllItem.Click += deleteAllItem_Click;

            // разделитель
            separator1.Index = 4;
            separator1.Text = "-";

            // обновить
            refreshItem.Index = 5;
            refreshItem.Text = "Обновить";
            refreshItem.Shortcut = Shortcut.F5;
            refreshItem.Click += refreshItem_Click;

            // разделитель
            separator2.Index = 6;
            separator2.Text = "-";

            // найти
            findItem.Index = 7;
            findItem.Text = "Найти...";
            findItem.Shortcut = Shortcut.CtrlF;
            findItem.Enabled = allowFind;
            findItem.Click += findItem_Click;

            // сортировать
            sortItem.Index = 8;
            sortItem.Text = "Упорядочить уровень";
            findItem.Enabled = allowMove;
            sortItem.Click += sortItem_Click;

            // обработчики событий...
            AfterLabelEdit += this_AfterLabelEdit;
            BeforeExpand += this_BeforeExpand;
            KeyUp += this_KeyUp;
            MouseDown += this_MouseDown;
            DragEnter += this_DragEnter;
            DragOver += this_DragOver;
            DragDrop += this_DragDrop;
            DragLeave += this_DragLeave;
            ContextMenu.Popup += ContextMenu_Popup;
            AfterSelect += this_AfterSelect;
        }
        
        #endregion

        #region Properties

        public Type NodeType
        {
            get { return nodeType; }
            set { nodeType = value; }
        }

        /// <summary>
        /// Строка подключения к БД, по умолчанию берётся из Kesco.Win.Data.Data.cnStr
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = (!string.IsNullOrEmpty(value)) ? value : null; }
        }

        /// <summary>
        /// Таблица откуда берутся данные
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        /// <summary>
        /// Ключевой столбец 
        /// </summary>
        public string IDField
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <summary>
        /// Описательный столбец (можно переопределить функцию )
        /// </summary>
        public string TextField
        {
            get { return textField; }
            set { textField = value; }
        }

        public string ParentField
        {
            get { return parentField; }
        }

        public string RightField
        {
            get { return rightField; }
        }

        public string LeftField
        {
            get { return leftField; }
        }

        /// <summary>
        /// Полностью или по частям будем загружать дерево
        /// </summary>
        public bool FullLoad
        {
            get { return fullLoad; }
            set { fullLoad = value; }
        }

        /// <summary>
        /// код текущего нода, 0 если нет выделенного
        /// </summary>
        public int CurrentId
        {
            get
            {
                int ret = 0;
                if (SelectedNode != null)
                    ret = SelectedNode.ID;
                return ret;
            }
        }

        public bool AllowAdd
        {
            get { return allowAdd; }
            set
            {
                allowAdd = value;
                newItem.Enabled = allowAdd;
            }
        }

        public bool AllowEdit
        {
            get { return allowEdit; }
            set
            {
                allowEdit = value;
                renameItem.Enabled = allowEdit;
            }
        }

        public bool AllowDelete
        {
            get { return allowDelete; }
            set
            {
                allowDelete = value;
                deleteItem.Enabled = allowDelete;
                deleteAllItem.Enabled = allowDelete;
            }
        }

        /// <summary>
        /// Можно выключить возможность перемещения по дереву
        /// </summary>
        public bool AllowMove
        {
            get { return allowMove; }
            set
            {
                allowMove = value;
                sortItem.Enabled = allowMove;
            }
        }

        public bool AllowFind
        {
            get { return allowFind; }
            set
            {
                allowFind = value;
                findItem.Enabled = allowFind;
            }
        }

        /// <summary>
        /// цвет нода, в который перемещаем
        /// </summary>
        public Color ColorMove
        {
            get { return colorMove; }
            set { colorMove = value; }
        }

        /// <summary>
        /// цвет нода на место которого перемещаем
        /// </summary>
        public Color ColorInsert
        {
            get { return colorInsert; }
            set { colorInsert = value; }
        }

        #endregion

        #region Find

        public DTreeNode FindNodeIn(TreeNodeCollection nds, int id)
        {
            for (int i = 0; i < nds.Count; i++)
            {
                var node = (DTreeNode) nds[i];
                if (id.Equals(node.ID))
                    return node;
            }
            return null;
        }

        /// <summary>
        /// Ищет ветку, по Id, при необходимости подгружая данные из базы
        /// </summary>
        /// <param name="id">код ветки</param>
        /// <returns></returns>
        public DTreeNode FindNode(int id)
        {
            DTreeNode nd;
            using (var dt = new DataTable())
            {
                string sql = " DECLARE @L int, @R  int\n" +
                             " SELECT @L=" + leftField + ",@R=" + rightField + " FROM " + tableName + " WHERE " +
                             idField + "=" + id + "\n" +
                             " SELECT " + idField + " FROM " + tableName +
                             " WHERE " + leftField + "<= ISNULL(@L,-1) AND " + rightField + ">= ISNULL(@R,-1) " +
                             " ORDER BY " + leftField;

                using (var da = new SqlDataAdapter(sql, ConnectionString))
                {
                    try
                    {
                        da.Fill(dt);
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show(sqlEx.Message, "Ошибка!");
                    }
                }

                if (dt.Rows.Count == 0) return null;

                TreeNodeCollection nds = Nodes;
                nd = null;
                DTreeNode pnd = null;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nd = FindNodeIn(nds, (int) dt.Rows[i][idField]);
                    if (nd == null)
                    {
                        RefreshSubNodes(pnd);
                        nd = FindNodeIn(nds, (int) dt.Rows[i][idField]);
                        if (nd == null) return null;
                    }
                    nds = nd.Nodes;
                    pnd = nd;
                }
            }
            return nd;
        }

        /// <summary>
        /// Открывает диалоговое окно поиска по дереву
        /// </summary>
        public void ShowFindDialog()
        {
            if (findDialog == null)
                findDialog = new DTreeFindDialog(this);

            findDialog.ShowDialog(FindForm());
        }

        /// <summary>
        /// Ищет следующую ветку, удовлетворяющую условию, заданному в диалоговом окне
        /// </summary>
        [Description("Ищет следующий от текущего, нод удовлетворяющий условию")]
        public bool FindNext(string searchString)
        {
            this.searchString = searchString;
            return FindNext();
        }

        public bool FindNext()
        {
            if (textField == null) return false;
            if (searchString.Length == 0)
                return false;
            string sql = "SELECT TOP 1 " + idField + " FROM " + tableName +
                         " WHERE " +
                         textField + " like '%" + searchString + "%' AND " +
                         leftField + "> ISNULL((SELECT " + leftField + " FROM " + tableName + " WHERE " + idField + "=" +
                         CurrentId + "),0)" +
                         " ORDER BY " + leftField;
            object rez;
            using (var cm = new SqlCommand(sql))
            {
                cm.Connection = new SqlConnection(ConnectionString);
                cm.Connection.Open();

                rez = cm.ExecuteScalar();
                cm.Connection.Close();
            }

            if (rez == null)
            {
                if (SelectedNode == null)
                {
                    MessageBox.Show("Не найдено");
                }
                else
                {
                    if (MessageBox.Show("Не найдено, начать с начала?", "", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        SelectedNode = null;
                        return FindNext();
                    }
                }
            }
            else
            {
                SelectNode((int) rez);
                return true;
            }
            return false;
        }

        #endregion

        #region Select

        /// <summary>
        /// Выбирает ветку по Id. <br/><see cref="FindNode"/>
        /// </summary>
        /// <param name="id"></param>
        public void SelectNode(int id)
        {
            DTreeNode nd = FindNode(id);
            if (nd == null) return;
            SelectedNode = nd;
        }

        public new DTreeNode SelectedNode
        {
            get { return (DTreeNode) base.SelectedNode; }
            set { base.SelectedNode = value; }
        }

        #endregion

        #region RefreshNode


        public virtual void RefreshNode(DTreeNode nd)
        {
            if (nd == null)
                return;
            using (var dt = new DataTable())
            {
                string sql;
                if (!string.IsNullOrEmpty(querySelect))
                {
                    sql = querySelect + " " + queryJoin;

                    if (!string.IsNullOrEmpty(queryWhere))
                        sql += " " + queryWhere + " AND " + idField + "=" + nd.ID;

                    sql += " " + queryOrderBy;
                }
                else
                {
                    sql = "SELECT * FROM " + tableName + " WHERE " + idField + "=" + nd.ID;
                }
                using (var da = new SqlDataAdapter(sql, ConnectionString))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count == 1)
                    RefreshNode(dt.Rows[0], nd);
            }
        }

        /// <summary>
        /// Обновляет нод, данными из таблицы
        /// </summary>
        /// <param name="row">запись таблицы</param>
        /// <param name="nd">нод, который нужно обновить</param>
        public virtual void RefreshNode(DataRow row, DTreeNode nd)
        {
            if (textField == "EmpSelect")
                textField = (row["КраткоеНазваниеРус"].ToString() != "") ? "КраткоеНазваниеРус" : "Кличка";
            if (textField != null) nd.Text = (string) row[textField];
            nd.ID = (int) row[idField];
            nd.Tag = row;
            OnFillNode(new FillNodeEventArgs(nd, row));

            if (fullLoad)
                return;
            if (nd.IsExpanded)
                return;
            if (nd.Nodes.Count == 0 && (int) row[rightField] - (int) row[leftField] > 1)
            {
                //подставляем заглушку
                var n = new DTreeNode();
                n.Text = ".";
                n.ID = 0;
                n.BackColor = Color.Gainsboro;
                nd.Nodes.Add(n);
            }
        }

        protected void OnFillNode(FillNodeEventArgs e)
        {
            if (FillNode != null)
                FillNode(this, e);
        }

        /// <summary>
        /// Обновляет подчиненные ветви нода
        /// </summary>
        /// <param name="nd">нод, чьи подчиненные ветви нужно обновить</param>
        public void RefreshSubNodes(DTreeNode nd)
        {
            TreeNodeCollection nds = (nd == null) ? Nodes : nd.Nodes;
            nds.Clear();
            LoadSubNodes(nd);
        }

        #endregion

        #region Fill

        public void Fill()
        {
            Nodes.Clear();
            LoadSubNodes(null);
        }

        private void LoadSubNodes(DTreeNode nd)
        {
            TreeNodeCollection nds = (nd == null) ? Nodes : nd.Nodes; //коллекция нодов, в которую будем загружать
            string filter = "[" + parentField + "]" + ((nd == null) ? " is null" : "=" + nd.ID); //фильтр, для определения корневых нодов, на текущем уровне
            string sql = !string.IsNullOrEmpty(queryString) ? queryString : "SELECT * FROM " + tableName; //sql запрос для выборки всех загружаемых нодов
            
            if (nd != null)
                sql = "DECLARE @L int,@R int\n" +
                      "SELECT @L=" + leftField + ",@R=" + rightField + " FROM " + tableName + " WHERE " + idField + "=" +
                      nd.ID + "\n" +
                      "SELECT * FROM (" + sql + ") AS T1 WHERE @L<=" + leftField + " AND " + rightField +
                      "<=@R ORDER BY " + leftField;

            if (string.IsNullOrEmpty(ConnectionString))
                return;
            using (var cmd = new SqlDataAdapter(sql, ConnectionString))
            using (var ds = new DataSet())
            {
                try
                {
                    cmd.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    DataColumn pk, fk;

                    pk = dt.Columns[idField];
                    fk = dt.Columns[parentField];
                    var rel = new DataRelation("ParentRelation", pk, fk, false);
                    ds.Relations.Add(rel);

                    DataRow[] drs = dt.Select(filter);
                    for (int i = 0; i < drs.Length; i++)
                        nds.Add(Populate(drs[i], rel));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + sql);
                }
            }
        }

        private DTreeNode Populate(DataRow dr, DataRelation rel)
        {
            var node = (DTreeNode) Activator.CreateInstance(nodeType);

            node.Tag = dr;
            RefreshNode(dr, node);

            if (fullLoad)
                foreach (DataRow row in dr.GetChildRows(rel))
                {
                    DTreeNode newNode = Populate(row, rel);
                    node.Nodes.Add(newNode);
                }
            return node;
        }

        #endregion

        #region Drag'n'Drop

        private void this_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine("{0}: tv AfterSelect -> {1}", DateTime.Now.ToString("HH:mm:ss fff"), ((e.Node != null) ? ((DTreeNode)e.Node).ID.ToString() : "null"));
        }

        private void this_MouseDown(object sender, MouseEventArgs e)
        {
            var nd = (DTreeNode) GetNodeAt(e.X, e.Y);
            var hit = this.HitTest(e.X, e.Y);
            if (hit.Location == TreeViewHitTestLocations.PlusMinus)
                return;

            if (hit.Location != TreeViewHitTestLocations.Label && hit.Location != TreeViewHitTestLocations.Image)
                nd = null;

            if (SelectedNode != nd)
            {
                SelectedNode = nd;
                if (SelectedNode == null)
                    OnAfterSelect(new TreeViewEventArgs(SelectedNode, TreeViewAction.Unknown));
            }
            if (allowMove && e.Button.Equals(MouseButtons.Left) && (SelectedNode != null))
            {
                var data = new DataObject();
                data.SetData(typeof (string), tableName + ":" + SelectedNode.ID + ":" + SelectedNode.Text);
                Console.WriteLine("{0}: DoDragDrop", DateTime.Now.ToString("HH:mm:ss fff"));
                DoDragDrop(data, DragDropEffects.Move);

            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                nd = (DTreeNode)GetNodeAt(e.X, e.Y);
                if (nd != null)
                {
                    nd.ForeColor = ForeColor;
                    nd.BackColor = BackColor;
                }
            }
        }

        private void this_DragEnter(object sender, DragEventArgs e)
        {
            if (!allowMove) return;
            string[] k = e.Data.GetData(typeof (string)).ToString().Split(':');
            if (k.Length > 2)
                if (k[0].Equals(tableName))
                    e.Effect = DragDropEffects.Move;
        }

        private void ExpandNode(object state)
        {
            if (dond != null)
            {
                if (dond.TreeView.InvokeRequired)
                    dond.TreeView.Invoke((MethodInvoker)(dond.Expand));
				else
                    dond.Expand();
            }
        }

        private void this_DragOver(object sender, DragEventArgs e)
        {
            Console.WriteLine("{0}: DragOver", DateTime.Now.ToString("HH:mm:ss fff"));
            if (!allowMove) return;
            string[] k = e.Data.GetData(typeof (string)).ToString().Split(':');
            int id = int.Parse(k[1]);

            var nd = (DTreeNode) GetNodeAt(PointToClient(new Point(e.X, e.Y)));

            var args = new CheckNodeForDropEventArgs();
            args.dropNode = nd;
            args.dragNode = SelectedNode;
            args.instead = (ModifierKeys == Keys.Control);
            if (CheckNodeForDrop != null) CheckNodeForDrop(this, args);

            e.Effect = args.canDrop ? DragDropEffects.Move : DragDropEffects.None;

            if ((nd != null) && (id.Equals(nd.ID)))
            {

                if (dond != null)
                {
                    dond.BackColor = BackColor;
                    dond = null;
                } //стираем предыдущую
                return;
            }

            if (nd != dond) //произошла смена dond
            {
                if (dond != null)
                    dond.BackColor = BackColor; //стираем предыдущую

                dond = nd;

                if (dond != null)
                {
                    if (dond.NextVisibleNode != null)
                        dond.NextVisibleNode.EnsureVisible();
                    if (dond.PrevVisibleNode != null)
                        dond.PrevVisibleNode.EnsureVisible();

                    if (dond.IsExpanded)
                        timer.Change(-1, -1);
                    else
                        timer.Change(1200, -1);
                }
            }

            if ((dond != null) && (args.canDrop))
                dond.BackColor = args.instead ? colorInsert : colorMove; //устанавливаем и подкрашиваем селедующую
        }

        private void this_DragDrop(object sender, DragEventArgs e)
        {
            if (!allowMove) return;
            Console.WriteLine("{0}: DragDrop", DateTime.Now.ToString("HH:mm:ss fff"));
            string[] k = e.Data.GetData(typeof (string)).ToString().Split(':');
            int src = int.Parse(k[1]);

            var nd = (DTreeNode) GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            int dst = (nd == null) ? 0 : nd.ID;

            if (src.Equals(dst))
                return;

            bool ins = (ModifierKeys == Keys.Control);
            if (ConfirmMovingMethod == null || ConfirmMovingMethod(k[2], (nd == null ? "" : nd.Text), ins))
            {
                MoveNode(src, dst, ins);
                OnRefreshNode();
                RefreshNode(SelectedNode);
                RefreshSubNodes(SelectedNode);
            }

            if (dond != null)
            {
                dond.BackColor = BackColor;
                dond = null;
            }
        }

        private void this_DragLeave(object sender, EventArgs e)
        {
            if (!allowMove)
                return;
            if (dond == null)
                return;

            dond.BackColor = BackColor;
            dond = null;
        }

        #endregion

        #region Edit

        private void this_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            LabelEdit = false;
            var nd = (DTreeNode) e.Node;
            if (nd.IsNew)
            {
                nd.Text = e.Label;
                var args = new TreeViewCancelEventArgs(nd, true, TreeViewAction.Unknown);
                OnAfterNewNodeEdit(args);
                if (args.Cancel)
                {
                    if (nd.Parent != null) SelectedNode = (DTreeNode) nd.Parent;
                    nd.Remove();
                }
                else
                {
                    nd.IsNew = false;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(e.Label) && e.Label != e.Node.Text)
                {
                    var node = (DTreeNode) e.Node;
                    if (!Rename(node.ID, e.Label))
                        e.CancelEdit = true;
                }
                else
                    e.CancelEdit = true;
            }
        }

        #endregion

        #region Context Menu

        public void ContextMenu_Popup(object sender, EventArgs e)
        {
            Point p = PointToClient(MousePosition);

            var node = (DTreeNode) GetNodeAt(p.X, p.Y);

            if (node != null)
            {
                SelectedNode = node;
                newItem.Enabled = allowAdd;
                renameItem.Enabled = allowEdit;
                deleteItem.Enabled = (node.Nodes.Count == 0) && allowDelete;
                deleteAllItem.Enabled = !deleteItem.Enabled && allowDelete;
                sortItem.Enabled = allowMove;
                findItem.Enabled = allowFind;
            }
            else
            {
                SelectedNode = null;
                newItem.Enabled = false;
                renameItem.Enabled = false;
                deleteItem.Enabled = false;
                deleteAllItem.Enabled = false;
                sortItem.Enabled = false;
                findItem.Enabled = false;
            }
        }

        public void AddNode()
        {
            // Добавляем новый нод(служебный)
            // переходим на него
            if ((SelectedNode != null) && (!SelectedNode.IsExpanded))
                SelectedNode.Expand();
            TreeNodeCollection nds = (SelectedNode != null) ? SelectedNode.Nodes : Nodes;
            var nd = (DTreeNode) Activator.CreateInstance(nodeType);
            nd.ID = 0;
            nd.IsNew = true;
            nds.Add(nd);
            SelectedNode = nd;
        }

        private void renameItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null)
                return;
            LabelEdit = true;
            SelectedNode.BeginEdit();
        }

        private void newItem_Click(object sender, EventArgs e)
        {
            AddNode();
            LabelEdit = true;
            SelectedNode.BeginEdit();
        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            DTreeNode node = SelectedNode;

            if (node != null && node.Nodes.Count == 0)
                if (
                    MessageBox.Show("Вы действительно хотите удалить выбранный узел?", "Подтверждение удаления",
                                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes)
                    if (Delete(node.ID))
                        node.Remove();
        }

        private void deleteAllItem_Click(object sender, EventArgs e)
        {
            DTreeNode node = SelectedNode;

            if (node != null)
                if (
                    MessageBox.Show("Вы действительно хотите удалить выбранный узел со всеми дочерними узлами?",
                                    "Подтверждение удаления", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    if (DeleteSubTree(node))
                        node.Remove();
        }

        private void findItem_Click(object sender, EventArgs e)
        {
            ShowFindDialog();
        }

        private void sortItem_Click(object sender, EventArgs e)
        {
            // достаем коллекцию узлов текущего уровня
            DTreeNode node = SelectedNode;
            TreeNodeCollection nodes = node.Parent != null ? node.Parent.Nodes : Nodes;

            // первый узел "добавлен"
            int count = 1;

            // пока не дошли до конца
            while (count < nodes.Count)
            {
                // следующий узел
                node = (DTreeNode) nodes[count];
                int i;

                // по всем узлам до текущего
                for (i = 0; i < count; i++)
                {
                    var curNode = (DTreeNode) nodes[i];
                    if (curNode.Text.CompareTo(node.Text) > 0)
                        break;
                }

                if (i < count)
                {
                    var curNode = (DTreeNode) nodes[i];
                    nodes.Remove(node);
                    nodes.Insert(i, node);
                    SortedMove(node.ID, curNode.ID);
                }

                count++;
            }
        }

        private void refreshItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null)
            {
                Nodes.Clear();
                Fill();
            }
            else
            {
                OnRefreshNode();
                if (textField != "EmpSelect")
                {
                    RefreshNode(SelectedNode);
                    RefreshSubNodes(SelectedNode);
                }
            }
        }

        #endregion

        #region Change Data

        public delegate bool ConfirmMoving(string src, string dst, bool ins);
        public ConfirmMoving ConfirmMovingMethod = null;

        /// <summary>
        /// Перемещает ветвь в БД, а затем в дереве <i>независимо от того как сработали тригеры результат будет соответствовать таблице</i>
        /// </summary>
        /// <param name="src">что перемещаем</param>
        /// <param name="dst">куда перемещаем</param>
        /// <param name="ins">true, если нужно переместить на место dst</param>
        public virtual void MoveNode(int src, int dst, bool ins)
        {
            if (!allowMove)
                return;
            if (src.Equals(dst))
                return;

            Console.WriteLine("{0}: MoveNode({1},{2},{3})", DateTime.Now.ToString("HH:mm:ss fff"), src, dst, ins);

            string sql = ins
                             ? " DECLARE @P int, @L int\n" +
                               " SELECT @P=" + parentField + ",@L=" + leftField + " FROM " + tableName + " WHERE " +
                               idField + "=" +
                               dst + "\n" +
                               " UPDATE " + tableName + " SET " +
                               parentField + "=@P," +
                               leftField + "=@L" +
                               " WHERE " + idField + "=" + src
                             : "UPDATE " + tableName + " SET " + parentField + "=" + dst + " WHERE " + idField + "=" +
                               src;

            using (var cm = new SqlCommand(sql))
            {
                cm.Connection = new SqlConnection(ConnectionString);
                try
                {
                    cm.Connection.Open();
                    cm.ExecuteNonQuery();
                    SelectedNode.Remove();
                    SelectNode(src);
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 229)
                        MessageBox.Show("У вас недостаточно прав для выполнения этой операции",
                                        "Ошибка: Отказано в доступе");
                    else
                        MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
                }
                finally
                {
                    cm.Connection.Close();
                }
            }
        }

        public bool Rename(int id, string text)
        {
            if (textField == null)
                return false;
            if (!allowEdit)
                return false;
            using (
                var cmd =
                    new SqlCommand(
                        "UPDATE " + tableName + " SET " + textField + " = @Text" + " WHERE " + idField + " = @ID",
                        new SqlConnection(ConnectionString)))
            {
                cmd.Parameters.Add(new SqlParameter("@Text", SqlDbType.NVarChar) {Value = text});
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) {Value = id} );

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 229)
                        MessageBox.Show("У вас недостаточно прав для выполнения этой операции",
                                        "Ошибка: Отказано в доступе");
                    else
                        MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
                    return false;
                }
            }

            return true;
        }

        public bool Delete(int id)
        {
            if (!allowDelete) return false;

            using (
                var cmd = new SqlCommand("DELETE FROM " + tableName + " WHERE " + idField + " = @ID",
                                         new SqlConnection(ConnectionString)))
            {
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) {Value = id});
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 229)
                        MessageBox.Show("У вас недостаточно прав для выполнения этой операции",
                                        "Ошибка: Отказано в доступе");
                    else
                        MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
                    return false;
                }
            }
            return true;
        }

        private bool DeleteSubTree(DTreeNode root)
        {
            bool result = true;
            for (int i = 0; i < root.Nodes.Count; i++)
            {
                var node = (DTreeNode) root.Nodes[i];
                result = DeleteSubTree(node);
                if (!result)
                    break;
            }

            if (result)
                result = Delete(root.ID);

            return result;
        }

        public bool UnsortedMove(int what, int where)
        {
            using (
                var cmd =
                    new SqlCommand(
                        "UPDATE " + tableName + " SET " + parentField + " = @ParentID" + " WHERE " + idField + " = @ID",
                        new SqlConnection(ConnectionString)))
            {
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@ParentID", SqlDbType.Int) {Value = @where});
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) {Value = what});
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 229)
                        MessageBox.Show("У вас недостаточно прав для выполнения этой операции",
                                        "Ошибка: Отказано в доступе");
                    else
                        MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
                    return false;
                }
            }

            return true;
        }

        public bool SortedMove(int what, int before)
        {
            using (
                var cmd =
                    new SqlCommand(
                        "UPDATE " + tableName + " SET " + parentField + " = (" + "SELECT " + parentField + " FROM " +
                        tableName + " WHERE " + idField + " = @BeforeID" + "), " + leftField + " = (" + "SELECT " +
                        leftField + " FROM " + tableName + " WHERE " + idField + " = @BeforeID)" + " WHERE " + idField +
                        " = @ID", new SqlConnection(ConnectionString)))
            {
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@BeforeID", SqlDbType.Int) {Value = before});
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) {Value = what});
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 229)
                        MessageBox.Show("У вас недостаточно прав для выполнения этой операции",
                                        "Ошибка: Отказано в доступе");
                    else
                        MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
                    return false;
                }
            }
            return true;
        }

        public bool Move4Sort(int what, int before)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = "UPDATE " + tableName + " SET " + leftField + " = (" + "SELECT " + leftField + " FROM " + tableName + " WHERE " + idField + " = @BeforeID)" + " WHERE " + idField + " = @ID";
                cmd.Connection = new SqlConnection(ConnectionString);
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@BeforeID", SqlDbType.Int) {Value = before});
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) {Value = what});
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 229)
                        MessageBox.Show("У вас недостаточно прав для выполнения этой операции", "Ошибка: Отказано в доступе");
                    else
                        MessageBox.Show(sqlEx.Message, "Ошибка доступа к базе данных");
                    return false;
                }
                finally
                {
                    if (cmd.Connection != null) cmd.Connection.Close();
                }
            }
            return true;
        }

        #endregion

        private void this_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!fullLoad && e.Node.Nodes.Count == 1 && e.Node.Nodes[0].BackColor.Equals(Color.Gainsboro))
                RefreshSubNodes((DTreeNode) e.Node);
        }

        public void SetQueryString(string queryString)
        {
            this.queryString = (queryString != null) && (queryString.Trim().Length) > 0 ? queryString : null;
        }

        public void SetQueryString(string querySelect, string queryJoin, string queryWhere, string queryOrderBy)
        {
            this.querySelect = querySelect;
            this.queryJoin = queryJoin;
            this.queryWhere = queryWhere;
            this.queryOrderBy = queryOrderBy;

            string q = querySelect + " " + queryJoin + " " + queryWhere + " " + queryOrderBy;

            queryString = q.Trim().Length > 0 ? q : null;
        }

        private void this_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    refreshItem_Click(this, EventArgs.Empty);
                    break;

                case Keys.F3:
                    if (allowFind)
                        FindNext();
                    break;

                case Keys.F:
                    if (e.Control && allowFind)
                        ShowFindDialog();
                    break;

                case Keys.F2:
                    if (allowEdit && renameItem.Enabled)
                        renameItem_Click(sender, e);
                    break;

                case Keys.Insert:
                    if (allowAdd && newItem.Enabled)
                        newItem_Click(sender, e);
                    break;

                case Keys.Delete:
                    if (e.Control)
                    {
                        if (allowDelete && deleteAllItem.Enabled)
                            deleteAllItem_Click(sender, e);
                    }
                    else
                    {
                        if (allowDelete && deleteItem.Enabled)
                            deleteItem_Click(sender, e);
                    }
                    break;
            }
        }
    }

    public delegate void FillNodeEventHandler(object sender, FillNodeEventArgs e);

    public class FillNodeEventArgs : EventArgs
    {
        public DTreeNode nd;
        public DataRow row;

        public FillNodeEventArgs(DTreeNode nd, DataRow row)
        {
            this.nd = nd;
            this.row = row;
        }
    }

    public delegate void CheckNodeForDropEventHandler(object sender, CheckNodeForDropEventArgs e);

    public class CheckNodeForDropEventArgs
    {
        public bool canDrop = true;
        public bool instead;
        public DTreeNode dropNode;
        public DTreeNode dragNode;
    }

    public delegate void RefreshNodeDelegate();
}
