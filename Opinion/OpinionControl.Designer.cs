namespace Kesco.Lib.Win.Opinion
{
	partial class OpinionControl
	{
		/// <summary> 
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary> 
		/// Обязательный метод для поддержки конструктора - не изменяйте 
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpinionControl));
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.buttonDislike = new System.Windows.Forms.Button();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.buttonLike = new System.Windows.Forms.Button();
			this.labelDislike = new System.Windows.Forms.Label();
			this.labelLike = new System.Windows.Forms.Label();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.AutoSize = true;
			this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel.ColumnCount = 4;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.labelLike, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.buttonLike, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.buttonDislike, 2, 0);
			this.tableLayoutPanel.Controls.Add(this.labelDislike, 3, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(44, 22);
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// buttonDislike
			// 
			this.buttonDislike.AutoSize = true;
			this.buttonDislike.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonDislike.FlatAppearance.BorderSize = 0;
			this.buttonDislike.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonDislike.ImageIndex = 2;
			this.buttonDislike.ImageList = this.imageList;
			this.buttonDislike.Location = new System.Drawing.Point(22, 0);
			this.buttonDislike.Margin = new System.Windows.Forms.Padding(0);
			this.buttonDislike.Name = "buttonDislike";
			this.buttonDislike.Size = new System.Drawing.Size(22, 22);
			this.buttonDislike.TabIndex = 1;
			this.buttonDislike.Click += new System.EventHandler(this.labelDislike_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "like_off.png");
			this.imageList.Images.SetKeyName(1, "like.png");
			this.imageList.Images.SetKeyName(2, "dislike_off.png");
			this.imageList.Images.SetKeyName(3, "dislike.png");
			// 
			// buttonLike
			// 
			this.buttonLike.AutoSize = true;
			this.buttonLike.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonLike.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonLike.FlatAppearance.BorderSize = 0;
			this.buttonLike.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonLike.ImageIndex = 0;
			this.buttonLike.ImageList = this.imageList;
			this.buttonLike.Location = new System.Drawing.Point(0, 0);
			this.buttonLike.Margin = new System.Windows.Forms.Padding(0);
			this.buttonLike.Name = "buttonLike";
			this.buttonLike.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.buttonLike.Size = new System.Drawing.Size(22, 22);
			this.buttonLike.TabIndex = 0;
			this.buttonLike.Click += new System.EventHandler(this.labelLike_Click);
			// 
			// labelDislike
			// 
			this.labelDislike.AutoSize = true;
			this.labelDislike.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelDislike.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
			this.labelDislike.Location = new System.Drawing.Point(44, 0);
			this.labelDislike.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.labelDislike.Name = "labelDislike";
			this.labelDislike.Size = new System.Drawing.Size(1, 22);
			this.labelDislike.TabIndex = 2;
			this.labelDislike.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// labelLike
			// 
			this.labelLike.AutoSize = true;
			this.labelLike.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelLike.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
			this.labelLike.Location = new System.Drawing.Point(0, 0);
			this.labelLike.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.labelLike.Name = "labelLike";
			this.labelLike.Size = new System.Drawing.Size(1, 22);
			this.labelLike.TabIndex = 3;
			this.labelLike.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// OpinionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "OpinionControl";
			this.Size = new System.Drawing.Size(44, 22);
			this.Load += new System.EventHandler(this.OpinionControl_Load);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Button buttonDislike;
		private System.Windows.Forms.Button buttonLike;
		private System.Windows.Forms.Label labelDislike;
		private System.Windows.Forms.Label labelLike;
	}
}
