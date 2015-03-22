using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Threader
{
	enum enumOneThread { A, B, C, D, E, F, G, H, K }
	enum enumStages { INIT, MAIN, FINISH, SUB1, SUB2, SUB3 }
	public partial class Form1 : Form
	{
		private Form2 taskform;
		private ThreadManager manager;
		public Form1()
		{
			InitializeComponent();
			this.Icon = Threader.Properties.Resources.threadicon;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			taskform = new Form2();

			List<Stage> dict = new List<Stage>();

			dict.Add(new Stage("INIT", new OneThread[]{
				new OneThread("A",()=>generate())
			}));
			dict.Add(new Stage("MAIN", new OneThread[]{
				new OneThread("B",()=>f1()),
				new OneThread("C",()=>f2())
			},
				new Stage[]{
					new Stage("SUB1",new OneThread[]{
						new OneThread("D",()=>f3())
				}),
					new Stage("SUB2",new OneThread[]{
						new OneThread("E",()=>f4()),
						new OneThread("F",()=>f5()),
						new OneThread("G",()=>f6())
				}),
					new Stage("SUB3",new OneThread[]{
						new OneThread("H",()=>f7())
				})
			}));
			dict.Add(new Stage("FINISH", new OneThread[]{
				new OneThread("K",()=>f8())
			}));

			manager = new ThreadManager(dict, this.textbox, this.statustreeview);
		}

		private void infobutton_Click(object sender, EventArgs e)
		{
			
		}

		private void taskbutton_Click(object sender, EventArgs e)
		{
			taskform.Show();
		}

		private void startbutton_Click(object sender, EventArgs e)
		{
			this.manager.Start();
		}

		private void stopbutton_Click(object sender, EventArgs e)
		{

		}

		private void savebutton_Click(object sender, EventArgs e)
		{
			this.textbox.Text = "";
			this.textbox.AppendText("CLICK:"+this.manager.Working().ToString());
		}

		public void generate()
		{
			Thread.Sleep(3000);
		}
		public void f1()
		{
			Thread.Sleep(9000);
		}
		public void f2()
		{
			Thread.Sleep(9000);
		}
		public void f3()
		{
			Thread.Sleep(3000);
		}
		public void f4()
		{
			Thread.Sleep(3000);
		}
		public void f5()
		{
			Thread.Sleep(3000);
		}
		public void f6()
		{
			Thread.Sleep(3000);
		}
		public void f7()
		{
			Thread.Sleep(3000);
		}
		public void f8()
		{
			Thread.Sleep(3000);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.manager.Working())
			{
				this.manager.Abort();
				Console.WriteLine(Monitor.TryEnter(textbox));
				//this.textbox.AppendText("status" + this.manager.Working().ToString() + "\n");
				while (this.manager.Working())
				{
					//this.textbox.AppendText("still working\n");
					Thread.Sleep(1000);
				}
				Thread.Sleep(1000);
			}
			this.taskform.Close();

		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Console.WriteLine(Monitor.TryEnter(textbox));
		}

	}
}
