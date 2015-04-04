namespace Threader
{
    partial class Form1
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.container = new System.Windows.Forms.ToolStripContainer();
			this.statustreeview = new System.Windows.Forms.TreeView();
			this.textbox = new System.Windows.Forms.RichTextBox();
			this.task = new System.Windows.Forms.Label();
			this.menu = new System.Windows.Forms.ToolStrip();
			this.startbutton = new System.Windows.Forms.ToolStripButton();
			this.savebutton = new System.Windows.Forms.ToolStripButton();
			this.taskbutton = new System.Windows.Forms.ToolStripButton();
			this.infobutton = new System.Windows.Forms.ToolStripButton();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.container.ContentPanel.SuspendLayout();
			this.container.TopToolStripPanel.SuspendLayout();
			this.container.SuspendLayout();
			this.menu.SuspendLayout();
			this.SuspendLayout();
			// 
			// container
			// 
			// 
			// container.ContentPanel
			// 
			this.container.ContentPanel.Controls.Add(this.statustreeview);
			this.container.ContentPanel.Controls.Add(this.textbox);
			this.container.ContentPanel.Controls.Add(this.task);
			this.container.ContentPanel.Size = new System.Drawing.Size(815, 330);
			this.container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.container.LeftToolStripPanelVisible = false;
			this.container.Location = new System.Drawing.Point(0, 0);
			this.container.Name = "container";
			this.container.RightToolStripPanelVisible = false;
			this.container.Size = new System.Drawing.Size(815, 401);
			this.container.TabIndex = 0;
			this.container.Text = "toolStripContainer1";
			// 
			// container.TopToolStripPanel
			// 
			this.container.TopToolStripPanel.Controls.Add(this.menu);
			// 
			// statustreeview
			// 
			this.statustreeview.Location = new System.Drawing.Point(14, 11);
			this.statustreeview.Name = "statustreeview";
			this.statustreeview.ShowPlusMinus = false;
			this.statustreeview.Size = new System.Drawing.Size(171, 307);
			this.statustreeview.TabIndex = 7;
			// 
			// textbox
			// 
			this.textbox.Location = new System.Drawing.Point(194, 47);
			this.textbox.MinimumSize = new System.Drawing.Size(331, 271);
			this.textbox.Name = "textbox";
			this.textbox.Size = new System.Drawing.Size(609, 271);
			this.textbox.TabIndex = 6;
			this.textbox.Text = "";
			// 
			// task
			// 
			this.task.AutoSize = true;
			this.task.Location = new System.Drawing.Point(333, 22);
			this.task.Name = "task";
			this.task.Size = new System.Drawing.Size(323, 13);
			this.task.TabIndex = 5;
			this.task.Text = "INIT(A)->MAIN(B,C,SUB1(D)->SUB2(E,F,G)->SUB3(H))->FINISH(K)";
			// 
			// menu
			// 
			this.menu.Dock = System.Windows.Forms.DockStyle.None;
			this.menu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.menu.ImageScalingSize = new System.Drawing.Size(64, 64);
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startbutton,
            this.savebutton,
            this.taskbutton,
            this.infobutton});
			this.menu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.menu.Location = new System.Drawing.Point(3, 0);
			this.menu.Name = "menu";
			this.menu.Size = new System.Drawing.Size(205, 71);
			this.menu.TabIndex = 1;
			// 
			// startbutton
			// 
			this.startbutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.startbutton.Image = global::Threader.Properties.Resources.start;
			this.startbutton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.startbutton.Name = "startbutton";
			this.startbutton.Size = new System.Drawing.Size(68, 68);
			this.startbutton.Text = "startbutton";
			this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
			// 
			// savebutton
			// 
			this.savebutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.savebutton.Image = global::Threader.Properties.Resources.save;
			this.savebutton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.savebutton.Name = "savebutton";
			this.savebutton.Size = new System.Drawing.Size(68, 68);
			this.savebutton.Text = "savebutton";
			this.savebutton.Click += new System.EventHandler(this.savebutton_Click);
			// 
			// taskbutton
			// 
			this.taskbutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.taskbutton.Image = global::Threader.Properties.Resources.task;
			this.taskbutton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.taskbutton.Name = "taskbutton";
			this.taskbutton.Size = new System.Drawing.Size(68, 68);
			this.taskbutton.Text = "taskbutton";
			this.taskbutton.Click += new System.EventHandler(this.taskbutton_Click);
			// 
			// infobutton
			// 
			this.infobutton.AutoSize = false;
			this.infobutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.infobutton.Image = global::Threader.Properties.Resources.info;
			this.infobutton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.infobutton.Name = "infobutton";
			this.infobutton.Size = new System.Drawing.Size(68, 68);
			this.infobutton.Text = "infobutton";
			this.infobutton.Visible = false;
			this.infobutton.Click += new System.EventHandler(this.infobutton_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.FileName = "Untitled.txt";
			this.saveFileDialog1.Filter = "Text file|*.txt";
			this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(815, 401);
			this.Controls.Add(this.container);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(831, 440);
			this.MinimumSize = new System.Drawing.Size(831, 440);
			this.Name = "Form1";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Threader";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.container.ContentPanel.ResumeLayout(false);
			this.container.ContentPanel.PerformLayout();
			this.container.TopToolStripPanel.ResumeLayout(false);
			this.container.TopToolStripPanel.PerformLayout();
			this.container.ResumeLayout(false);
			this.container.PerformLayout();
			this.menu.ResumeLayout(false);
			this.menu.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.ToolStripContainer container;
		private System.Windows.Forms.Label task;
		private System.Windows.Forms.ToolStrip menu;
		private System.Windows.Forms.ToolStripButton startbutton;
		private System.Windows.Forms.ToolStripButton savebutton;
		private System.Windows.Forms.ToolStripButton infobutton;
		private System.Windows.Forms.RichTextBox textbox;
		private System.Windows.Forms.ToolStripButton taskbutton;
		private System.Windows.Forms.TreeView statustreeview;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;

	}
}

