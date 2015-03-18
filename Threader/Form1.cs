using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Threader
{
	enum Stages { INIT, MAIN, SUB1, SUB2, SUB3, FINISH }
	public partial class Form1 : Form
	{
		private Form2 taskform;
		public Form1()
		{
			InitializeComponent();
            this.Icon = Threader.Properties.Resources.threadicon;
			for (char c = 'A'; c <= 'K'; c++)
			{
				if(c != 'J' && c != 'I')
					this.tasklist.Items.Add(c);
			}
			for (Stages s = Stages.INIT; s <= Stages.FINISH; s++)
			{
				this.stages.Items.Add(s);
			}
			taskform = new Form2();
		}

		private void infobutton_Click(object sender, EventArgs e)
		{
			
		}

		private void taskbutton_Click(object sender, EventArgs e)
		{
			taskform.Show();
		}

	}
}
