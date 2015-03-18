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
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.task = new System.Windows.Forms.Label();
			this.stages = new System.Windows.Forms.CheckedListBox();
			this.tasklist = new System.Windows.Forms.CheckedListBox();
			this.menu = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.savebutton = new System.Windows.Forms.ToolStripButton();
			this.taskbutton = new System.Windows.Forms.ToolStripButton();
			this.infobutton = new System.Windows.Forms.ToolStripButton();
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
			this.container.ContentPanel.Controls.Add(this.richTextBox1);
			this.container.ContentPanel.Controls.Add(this.task);
			this.container.ContentPanel.Controls.Add(this.stages);
			this.container.ContentPanel.Controls.Add(this.tasklist);
			this.container.ContentPanel.Size = new System.Drawing.Size(540, 274);
			this.container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.container.LeftToolStripPanelVisible = false;
			this.container.Location = new System.Drawing.Point(0, 0);
			this.container.Name = "container";
			this.container.RightToolStripPanelVisible = false;
			this.container.Size = new System.Drawing.Size(540, 345);
			this.container.TabIndex = 0;
			this.container.Text = "toolStripContainer1";
			// 
			// container.TopToolStripPanel
			// 
			this.container.TopToolStripPanel.Controls.Add(this.menu);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(174, 47);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(351, 214);
			this.richTextBox1.TabIndex = 6;
			this.richTextBox1.Text = "";
			// 
			// task
			// 
			this.task.AutoSize = true;
			this.task.Location = new System.Drawing.Point(191, 21);
			this.task.Name = "task";
			this.task.Size = new System.Drawing.Size(323, 13);
			this.task.TabIndex = 5;
			this.task.Text = "INIT(A)->MAIN(B,C,SUB1(D)->SUB2(E,F,G)->SUB3(H))->FINISH(K)";
			// 
			// stages
			// 
			this.stages.FormattingEnabled = true;
			this.stages.Location = new System.Drawing.Point(19, 168);
			this.stages.Name = "stages";
			this.stages.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.stages.Size = new System.Drawing.Size(142, 94);
			this.stages.TabIndex = 3;
			// 
			// tasklist
			// 
			this.tasklist.FormattingEnabled = true;
			this.tasklist.Location = new System.Drawing.Point(19, 16);
			this.tasklist.Name = "tasklist";
			this.tasklist.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.tasklist.Size = new System.Drawing.Size(142, 139);
			this.tasklist.TabIndex = 4;
			// 
			// menu
			// 
			this.menu.Dock = System.Windows.Forms.DockStyle.None;
			this.menu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.menu.ImageScalingSize = new System.Drawing.Size(64, 64);
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.savebutton,
            this.taskbutton,
            this.infobutton});
			this.menu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.menu.Location = new System.Drawing.Point(3, 0);
			this.menu.Name = "menu";
			this.menu.Size = new System.Drawing.Size(372, 71);
			this.menu.TabIndex = 1;
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::Threader.Properties.Resources.start;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(68, 68);
			this.toolStripButton1.Text = "startbutton";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::Threader.Properties.Resources.stop;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(68, 68);
			this.toolStripButton2.Text = "stopbutton";
			// 
			// savebutton
			// 
			this.savebutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.savebutton.Image = global::Threader.Properties.Resources.save;
			this.savebutton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.savebutton.Name = "savebutton";
			this.savebutton.Size = new System.Drawing.Size(68, 68);
			this.savebutton.Text = "savebutton";
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(540, 345);
			this.Controls.Add(this.container);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(556, 384);
			this.MinimumSize = new System.Drawing.Size(556, 384);
			this.Name = "Form1";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Threader";
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
		private System.Windows.Forms.CheckedListBox stages;
		private System.Windows.Forms.CheckedListBox tasklist;
		private System.Windows.Forms.ToolStrip menu;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripButton savebutton;
		private System.Windows.Forms.ToolStripButton infobutton;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ToolStripButton taskbutton;

	}
}

